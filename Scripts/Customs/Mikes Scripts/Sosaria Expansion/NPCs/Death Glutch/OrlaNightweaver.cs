using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WebOfTheUnseenQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Web of the Unseen"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Orla Nightweaver*, her presence a chilling mix of poise and menace.\n\n" +
                    "Clad in dark robes laced with crimson threads, she holds a **shard of blackened glass**, its surface flickering with spectral images.\n\n" +
                    "“This shard was once whole—a mirror used by the Hexlord beneath Malidor’s fallen halls. It no longer shows reflections, only intentions. Dark ones.”\n\n" +
                    "“The covens tread carefully now. If we let him weave unchecked, his spells will snare not just the Academy but all of Death Glutch in his madness.”\n\n" +
                    "“This is not an open war… not yet. But **cut the web at its heart**, and we might still keep the balance.”\n\n" +
                    "**Find the Hexlord** beneath the Academy wings. Slay him before his magic forces our hand and ignites a blood feud none will survive.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the weave tightening around us. I’ll hold the covens off… but not forever.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The threads grow restless. His web stretches beyond the Academy now. Are you too caught already?";
            }
        }

        public override object Complete
        {
            get
            {
                return "The shard quiets... the web is torn, and his spell is broken.\n\n" +
                       "You have done more than slay a warlock—you’ve held a fragile peace.\n\n" +
                       "Take this: *Bladedancer’s Helm.* It suits one who knows when to strike, and when to wait.";
            }
        }

        public WebOfTheUnseenQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Hexlord), "the Hexlord", 1));
            AddReward(new BaseReward(typeof(BladedancersCloseHelm), 1, "Bladedancer's Helm"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Web of the Unseen'!");
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

    public class OrlaNightweaver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WebOfTheUnseenQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer()); // Closest fit to a Cult Liaison
        }

        [Constructable]
        public OrlaNightweaver()
            : base("the Cult Liaison", "Orla Nightweaver")
        {
        }

        public OrlaNightweaver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1102; // Jet black
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1109, Name = "Nightweaver's Robe" }); // Deep shadow grey
            AddItem(new Cloak() { Hue = 1157, Name = "Crimson-Threaded Cloak" }); // Blood red hue
            AddItem(new FeatheredHat() { Hue = 1102, Name = "Whisperer's Veil" }); // Jet black with ritualistic feel
            AddItem(new Sandals() { Hue = 1109, Name = "Silent Step Sandals" });

            AddItem(new Scepter() { Hue = 1175, Name = "Shard of the Ritual Mirror" }); // Pale purple tint for arcane feel

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Weaver's Satchel";
            AddItem(backpack);
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
}
