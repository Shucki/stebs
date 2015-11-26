﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using assembler.support;
using assembler;
using System.IO;
using Microsoft.Practices.Unity;
using ProcessorSimulation.MpmParser;
using ProcessorSimulation;
using System.Threading.Tasks;

namespace Stebs5
{
    public class StebsHub : Hub
    {
        private IConstants Constants { get; }
        private IMpm Mpm { get; }
        private IProcessorManager Manager { get; }

        public StebsHub(IConstants constants, IMpm mpm, IProcessorManager manager)
        {
            this.Constants = constants;
            this.Mpm = mpm;
            this.Manager = manager;
        }

        public override Task OnConnected()
        {
            Manager.CreateProcessor(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Manager.AssureProcessorExists(Context.ConnectionId);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Manager.RemoveProcessor(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public void Assemble(string source)
        {
            lock (typeof(Common))
            {
                Assembler assembler = new Assembler(string.Empty);
                if (assembler.execute(source, Mpm.RawInstructions))
                {
                    Clients.Caller.Assembled(Common.getCodeList().toString(), Common.getRam(), assembler.getCodeToLineArr());
                    Manager.ChangeRamContent(Context.ConnectionId, Common.getRam());
                }
                else
                {
                    Clients.Caller.AssembleError(Common.ERROR_MESSAGE);
                }
            }
        }

        public void GetInstructions()
        {
            Clients.Caller.Instructions(Mpm.Instructions);
        }

        public void AddFile(int parentId, string fileName)
        {
            //List<Tuple<int, int, string>>
            Clients.Caller.SetFileId(parentId + 10);
        }
        public void AddFolder(int parentId, string fileName)
        {
            Clients.Caller.SetFileId(parentId + 10);
        }

        public void LoadRegisters()
        {
            Clients.Caller.SetAvailableRegisters(((Registers[])Enum.GetValues(typeof(Registers)))
                .Select(type => type.ToString()));
        }
    }
}
