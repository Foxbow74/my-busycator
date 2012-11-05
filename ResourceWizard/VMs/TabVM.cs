using ClientCommonWpf;

namespace ResourceWizard.VMs
{
	abstract class TabVM: AbstractViewModel
	{
		public abstract string DisplayName { get; }
	}
}