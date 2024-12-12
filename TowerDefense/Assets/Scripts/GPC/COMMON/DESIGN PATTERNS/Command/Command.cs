using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Harris.GPC
{

    public interface IInputReceiver
    {
        public void ReceiveCommand(InputCommand command);  

        //event called by keyboard input system to bind receivers to specific input commands 
        public static event Action<string, IInputReceiver> _onBindReceiver;

        public void BindReceiver(string cmdType)
        {
            _onBindReceiver?.Invoke(cmdType, this);
        }

    }


    public abstract class InputCommand
    {
        public InputAction Action{get;set;}

        enum ButtonState
        {
            None, Pressed, PressedThisFrame, Released
        }
        private ButtonState buttonState{get;set;}

        public bool Triggered => buttonState == ButtonState.PressedThisFrame;

        public bool Executing => buttonState == ButtonState.Pressed;

        public bool Stopped => buttonState == ButtonState.Released;

		private IInputReceiver _receiver;
        public IInputReceiver Receiver => _receiver;

        public void SetReceiver(IInputReceiver receiver)
		{
			_receiver = receiver;
		}

        public InputCommand(InputAction action)
        {
            Action = action;
            action.performed += CommandPerformed;
            action.canceled += CommandCanceled;
            buttonState = ButtonState.None;
        }

        public virtual void Execute()
        {
            buttonState = ButtonState.Pressed;
        }

        public virtual void CommandPerformed(InputAction.CallbackContext obj)
        {
            buttonState = ButtonState.PressedThisFrame;
            _receiver.ReceiveCommand(this);
        }

        public virtual void CommandCanceled(InputAction.CallbackContext obj)
        {
            buttonState = ButtonState.Released;
            _receiver.ReceiveCommand(this);
        }
    }

    public class WASDCommand : InputCommand
    {
        public WASDCommand(InputAction action) : base(action) {}

        public override void CommandPerformed(InputAction.CallbackContext obj)
        {
            base.CommandPerformed(obj);
            Debug.Log("Move Button Pressed This Frame!");
        }

        public override void CommandCanceled(InputAction.CallbackContext obj)
        {
            base.CommandCanceled(obj);
            Debug.Log("Move Button Released This Frame!");
        }

        public override void Execute()
        {
            base.Execute();
            Debug.Log("Move Command executed!");
            Receiver.ReceiveCommand(this);
        }
    }

    public class JumpCommand:  InputCommand
    {
        public JumpCommand(InputAction action) : base(action) {}

        public override void CommandPerformed(InputAction.CallbackContext obj)
        {
            base.CommandPerformed(obj);
            Debug.Log("Jump Button Pressed This Frame!");
        }

        public override void CommandCanceled(InputAction.CallbackContext obj)
        {
            base.CommandCanceled(obj);
        }

        public override void Execute()
        {
            base.Execute();
        }
    }


    public class SprintCommand:  InputCommand
    {
        public SprintCommand(InputAction action) : base(action) {}

        public override void CommandPerformed(InputAction.CallbackContext obj)
        {
            base.CommandPerformed(obj);
        }

        public override void CommandCanceled(InputAction.CallbackContext obj)
        {
            base.CommandCanceled(obj);
        }
        public override void Execute()
        {
            base.Execute();
        }
    }


    public class InteractCommand: InputCommand 
    {
        public InteractCommand(InputAction action) : base(action) {}

        public override void Execute()
        {
            
        }
    }

    public class MissionLogToggleCommand:  InputCommand
    {
        public MissionLogToggleCommand(InputAction action) : base(action) {}
        public override void Execute()
        {
            
        }
    }


    public class InventoryToggleCommand: InputCommand
    {

        public InventoryToggleCommand(InputAction action) : base(action) {}
        public override void Execute()
        {
            
        }
    }


    public class ShiftCommand:  InputCommand
    {
        public ShiftCommand(InputAction action) : base(action) {}
        public override void Execute()
        {
            
        }
    }
}


