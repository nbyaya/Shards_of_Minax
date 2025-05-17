using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ThornboundMadnessQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Thornbound Madness"; } }

        public override object Description
        {
            get
            {
                return
                    "The sands whisper in roots now, traveler.\n\n" +
                    "I am Jorin Brightquill, Scholar of the Sands, and I tend to Moon's oasis. A vile blight has taken hold—a " +
                    "CursedTree, grown from madness, leeching the life of our waters.\n\n" +
                    "**Destroy the Cursed Tree** that festers within the oasis, before it bends the minds of those who drink. " +
                    "Beware—its bark drips with dreams and poison alike.";
            }
        }

        public override object Refuse { get { return "Then beware the water, stranger. The oasis will not wait for heroes."; } }

        public override object Uncomplete { get { return "The CursedTree still thrives. The oasis wilts beneath its shadow."; } }

        public override object Complete { get { return "The roots wither, and the oasis breathes anew. Take this gift—mind and flame, for one who faced madness."; } }

        public ThornboundMadnessQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedTree), "CursedTree", 1));

            AddReward(new BaseReward(typeof(MindflameVisageOfDrakkon), 1, "MindflameVisageOfDrakkon"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Thornbound Madness'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class JorinBrightquill : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ThornboundMadnessQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBProvisioner()); 
        }

        [Constructable]
        public JorinBrightquill() : base("Jorin Brightquill", "Scholar of the Sands")
        {
            Title = "Scholar of the Sands";
			Body = 0x190; // Male human

            // Outfit - Evocative of lunar scholars, desert mystics, and botanical studies.
            AddItem(new Robe { Hue = 1153, Name = "Moonshadow Robe" }); // Deep midnight blue, star-studded trim
            AddItem(new Sandals { Hue = 2401, Name = "Sandwalker's Steps" }); // Pale sandstone hue
            AddItem(new WizardsHat { Hue = 1150, Name = "Brightquill’s Cowl" }); // Silver-laced, reflecting moonlight
            AddItem(new BodySash { Hue = 1160, Name = "Sash of Oasis Bloom" }); // Light green, symbolizing life amidst the sands
            AddItem(new QuarterStaff { Hue = 2424, Name = "Rootbinder Staff" }); // Etched with thorn and vine patterns, polished wood

            // Stats (Not a fighter, but resilient for lore-rich presence)
            SetStr(60, 70);
            SetDex(70, 80);
            SetInt(100, 120);

            SetDamage(3, 5);
            SetHits(150, 180);
        }

        public JorinBrightquill(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
