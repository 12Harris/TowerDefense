using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Harris.GPC;

namespace TowerDefense
{

	[AddComponentMenu("CSharpBookCode/Common/Keyboard Input Controller")]

	public class KeyboardInput : BaseKeyboardInput
	{

   	 	private InputCommand wasdCmd;
		public InputCommand WasdCmd => wasdCmd;

        public Vector2 wasdInput => WasdCmd.Action.ReadValue<Vector2>();

		private void ConnectReceiver(string cmdType, IInputReceiver receiver)
		{
			if(cmdType == "WASDCmd")
				wasdCmd.SetReceiver(receiver);
		}

		public void Initialize()
		{
            wasdCmd = new WASDCommand(wasdAction);
            wasdAction.Enable();
			IInputReceiver._onBindReceiver += ConnectReceiver;
		}

		private void OnDisable()
		{
			wasdCmd.Action.Disable();
		}

		public void Update()
		{
			if(wasdInput != Vector2.zero)
			{
				wasdCmd.Execute();
			}
		}
	}
}