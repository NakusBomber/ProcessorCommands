﻿using ProcessorCommands.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcessorCommands.Models.ProcessorCommands
{
    public class RR : TwoByteProcessorCommand
    {
        public RR(MainViewModel vm) : base(vm)
        {
        }

        public override int MaxStep {
            get
            {
                if(Type == ETypeCommand.Arithmetic)
                {
                    return 6;
                }
                return 4;
            }
        }

        public override ECommands Command => ECommands.RR;

        public override List<string> GetErrorsValues()
        {
            var errors = new List<string>();

            var intAddress = Convert.ToInt32(_vm.CounterAddress.Value, 16);
            var firstByte = _vm.RAM[intAddress].Value;
            var typeCommand = _vm.processor.GetTypeCommand(firstByte);
            var isFirstSavePlace = _vm.processor.isFirstPlaceSaveResult(firstByte);

            if (!SupportedTypes.Contains(typeCommand))
            {
                errors.Add("This type of command does not exist");
                return errors;
            }
            
            if (intAddress >= 255 || _vm.RAM[intAddress+1].Value == string.Empty)
            {
                errors.Add("Not found second byte command");
                return errors;
            }

            var secondByte = _vm.RAM[intAddress + 1].Value;
            var value1 = _vm.DataRegisters[_vm.processor.GetIndexDataRegister(secondByte, true)].Value;
            if(typeCommand == ETypeCommand.Arithmetic || 
                (typeCommand == ETypeCommand.Delivery && !isFirstSavePlace))
            {
                if (value1 == string.Empty)
                {
                    errors.Add("Empty first value");
                }
            }
            

            var value2 = _vm.DataRegisters[_vm.processor.GetIndexDataRegister(secondByte, false)].Value;
            if (typeCommand == ETypeCommand.Arithmetic ||
                (typeCommand == ETypeCommand.Delivery && isFirstSavePlace))
            {
                if (value2 == string.Empty)
                {
                    errors.Add("Empty second value");
                }
            }
            
            return errors;
        }

        protected override async Task ArithmeticAlgorithm()
        {
            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);
                    break;
                case 2:
                    await SampleOperand(0);
                    break;
                case 3:
                    await SampleOperand(1);
                    break;
                case 4:
                    await ExecuteArithmetic();
                    break;
                case 5:
                    await Save();
                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
        }

        protected override async Task DeliveryAlgorithm()
        {
            switch (_vm.Step)
            {
                case 1:
                    await SampleByteCommand(1);
                    break;
                case 2:
                    await ExecuteDelivery();
                    break;
                case 3:
                    await Delay(400);
                    Finish();
                    break;
                default:
                    break;
            }
        }

        protected override async Task ExecuteDelivery()
        {
            _vm.Status = ProgramStatus.ExecutionCommand;

            Token.ThrowIfCancellationRequested();

            var firstByte = _vm.CommandRegister.Value.Split()[0];
            var secondByte = _vm.CommandRegister.Value.Split()[1];
            var isFirstSavePlace = _vm.processor.isFirstPlaceSaveResult(firstByte);

            var firstIndex = _vm.processor.GetIndexDataRegister(secondByte, true);
            var secondIndex = _vm.processor.GetIndexDataRegister(secondByte, false);

            var saveIndex = isFirstSavePlace ? firstIndex : secondIndex;
            var dataIndex = isFirstSavePlace ? secondIndex : firstIndex;

            await _vm.DataRegisters[dataIndex].Animation();
            await _vm.DataRegisters[saveIndex].Animation();

            Token.ThrowIfCancellationRequested();
            _vm.DataRegisters[saveIndex].Value = _vm.DataRegisters[dataIndex].Value;

            _vm.Step++;
        }
        protected override async Task SampleOperand(int numberOperand)
        {
            Token.ThrowIfCancellationRequested();

            switch (numberOperand)
            {
                case 0:
                    _vm.Status = ProgramStatus.Sample1Operand;
                    break;
                case 1:
                    _vm.Status = ProgramStatus.Sample2Operand;
                    break;
                default:
                    return;
            }

            var register = _vm.processor.GetIndexDataRegister(_vm.CommandRegister.Value.Split()[1], numberOperand == 0);
            await _vm.DataRegisters[register].Animation();

            Token.ThrowIfCancellationRequested();

            if (numberOperand == 0)
            {
                await _vm.AluFirstRegister.Animation();
                _vm.AluFirstRegister.Value = _vm.DataRegisters[register].Value;
            }
            else
            {
                await _vm.AluSecondRegister.Animation();
                _vm.AluSecondRegister.Value = _vm.DataRegisters[register].Value;
            }
            _vm.Step++;
        }
        protected override async Task Save()
        {
            _vm.Status = ProgramStatus.SaveResult;

            Token.ThrowIfCancellationRequested();

            var firstByte = _vm.CommandRegister.Value.Split()[0];
            var secondByte = _vm.CommandRegister.Value.Split()[1];
            var isSaveFirstPlace = _vm.processor.isFirstPlaceSaveResult(firstByte);
            var place = _vm.processor.GetIndexDataRegister(secondByte, isSaveFirstPlace);
            await _vm.ResultRegister.Animation();
            await _vm.DataRegisters[place].Animation();
            _vm.DataRegisters[place].Value = _vm.ResultRegister.Value;

            _vm.Step++;
        }
    }
}
