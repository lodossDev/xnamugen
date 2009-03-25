using System;
using System.Diagnostics;
using xnaMugen.IO;
using System.Collections.Generic;

namespace xnaMugen.StateMachine.Controllers
{
	[StateControllerName("DisplayToClipboard")]
	class DisplayToClipboard : StateController
	{
		public DisplayToClipboard(StateSystem statesystem, String label, TextSection textsection)
			: base(statesystem, label, textsection)
		{
			m_formatstring = textsection.GetAttribute<String>("text", null);
			m_params = textsection.GetAttribute<Evaluation.Expression>("params", null);

			m_result = new Evaluation.Result();
		}

		public override void Run(Combat.Character character)
		{
			if (Parameters != null)
			{
				Parameters.Evaluate(character, m_result);

				character.Clipboard.Length = 0;
				character.Clipboard.Append(BuildString(m_result));
			}
			else
			{
				character.Clipboard.Length = 0;
				character.Clipboard.Append(FormatString);
			}
		}

		protected String BuildString(Evaluation.Result args)
		{
			if (args == null) throw new ArgumentNullException("args");

			return StateSystem.GetSubSystem<StringFormatter>().BuildString(FormatString, args);
		}

		public override Boolean IsValid()
		{
			if (base.IsValid() == false) return false;

			if (FormatString == null) return false;

			return true;
		}

		public String FormatString
		{
			get { return m_formatstring; }
		}

		public Evaluation.Expression Parameters
		{
			get { return m_params; }
		}

		#region Fields

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly String m_formatstring;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly Evaluation.Expression m_params;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		readonly Evaluation.Result m_result;

		#endregion
	}
}