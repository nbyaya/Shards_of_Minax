using System;
using Server.Items;

namespace Server.Items
{
    public class BraceletOfNaturesBounty : GoldBracelet // Inherits from GoldBracelet, you can change this if you want
    {
        private Timer m_Timer;

        [Constructable]
        public BraceletOfNaturesBounty() : base()
        {
            Name = "Bracelet of Nature's Bounty";
            Hue = 0x48E; // Green hue, can be adjusted

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), GenerateReagent); // Calls the GenerateReagent method every 10 seconds
            m_Timer.Start();
        }

        private void GenerateReagent()
        {
            if (this.Parent is Mobile && this.IsChildOf(((Mobile)this.Parent).Backpack)) // Checks if the bracelet is in a player's backpack
            {
                BaseReagent reagent = null;
                switch (Utility.Random(5)) // Randomly selects a reagent to generate
                {
                    case 0: reagent = new BlackPearl(); break;
                    case 1: reagent = new Bloodmoss(); break;
                    case 2: reagent = new MandrakeRoot(); break;
                    case 3: reagent = new Ginseng(); break;
                    case 4: reagent = new Garlic(); break;
                }
                if (reagent != null)
                {
                    ((Mobile)this.Parent).AddToBackpack(reagent);
                    ((Mobile)this.Parent).SendMessage("The bracelet generates a reagent!");
                }
            }
        }

        public BraceletOfNaturesBounty(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), GenerateReagent);
            m_Timer.Start();
        }
    }
}
