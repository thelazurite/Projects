using Gtk;

namespace Projects.main
{
	class Test : VBox
	{
		public Entry Entry1;
		public Test()
		{
			Entry1 = new Entry();
			Add(Entry1);
			var entryChild = ((BoxChild) (this[Entry1]));
			entryChild.Expand = true;
		}
	}
}
