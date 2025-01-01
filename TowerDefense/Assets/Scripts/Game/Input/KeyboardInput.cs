using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Harris.GPC;

namespace TowerDefense
{

	[AddComponentMenu("CSharpBookCode/Common/Keyboard Input Controller Game")]

	public class KeyboardInput : BaseKeyboardInput
	{

   	 	private Command wasdCmd;
		public Command WasdCmd => wasdCmd;

		private Command lShiftCommand;
		public Command LShiftCommand => lShiftCommand;

        public Vector2 wasdInput => WasdCmd.Action.ReadValue<Vector2>();

		private void ConnectReceiver(string cmdType, ICommandReceiver receiver)
		{
			if(cmdType == "WASDCmd")
				wasdCmd.SetReceiver(receiver);

			else if(cmdType == "LShiftCmd")
				LShiftCommand.SetReceiver(receiver);
		}

		public override void Initialize()
		{
			base.Initialize();
            wasdCmd = new WASDCommand(wasdAction);
			lShiftCommand = new Command(lShiftBtnAction, "LShiftCmd");
			lShiftBtnAction.Enable();
            wasdAction.Enable();
			ICommandReceiver._onBindReceiver += ConnectReceiver;
		}

		private void OnDisable()
		{
			wasdCmd.Action.Disable();
		}

		public void Update()
		{

			lShiftCommand.Update(Time.deltaTime);

			if(wasdInput != Vector2.zero)
			{
				wasdCmd.Execute();
			}
		}
	}
}