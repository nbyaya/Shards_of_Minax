using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;


namespace Server.Engines.Quests
{
    public class SilentIncantationQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Incantation"; } }

        public override object Description
        {
            get
            {
                return
                    "Cortrin Ashvale, Scholar of Yew’s Archive, stands amidst shelves of aged tomes and fading scrolls.\n\n" +
                    "His robes shimmer with dust motes, hands trembling not with fear, but urgency.\n\n" +
                    "“The vaults beneath Catastrophe… they hum with knowledge, but they *consume* the unwary.”\n\n" +
                    "“My mentor, Eldric, perished there—seeking a tome held by the **DecayedMage**. He believed the text spoke of warding the Void’s touch.”\n\n" +
                    "“If that knowledge is lost, we risk more than ignorance. We risk *repetition*—the same darkness, rising anew.”\n\n" +
                    "“I cannot lose this. Not again. Please, recover the tome before Catastrophe buries it in silence forever.”\n\n" +
                    "**Slay the DecayedMage**, and return with the arcane tome he guards.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may memory fade with the silence. I will pray the vaults don’t claim another.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no word? I see his face in the flicker of candlelight, begging me not to let his death be in vain.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done what I could not.\n\n" +
                       "This tome… its pages throb with the weight of forgotten wards. Eldric would have smiled.\n\n" +
                       "Take this: *BlightTouchedWarhelm.* Let it shield you where knowledge alone cannot.";
            }
        }

        public SilentIncantationQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DecayedMage), "DecayedMage", 1));
            AddReward(new BaseReward(typeof(BlightTouchedWarhelm), 1, "BlightTouchedWarhelm"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Incantation'!");
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

    public class CortrinAshvale : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilentIncantationQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }		

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBScribe( this ) );
		}


        [Constructable]
        public CortrinAshvale()
            : base("the Archive Scholar", "Cortrin Ashvale")
        {
        }

        public CortrinAshvale(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Ash-blonde
            FacialHairItemID = 0x203E; // Short beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 2602, Name = "Twilight Scholar's Robe" }); // Deep violet
            AddItem(new Shoes() { Hue = 1908, Name = "Vault-Treader's Shoes" }); // Dusty gray
            AddItem(new BodySash() { Hue = 1153, Name = "Ink-Stained Sash" }); // Midnight blue
            AddItem(new WizardsHat() { Hue = 2603, Name = "Ashvale's Pointed Cap" }); // Dark indigo
            AddItem(new Backpack() { Hue = 1150, Name = "Satchel of Lore" });

            AddItem(new ScribeSword() { Hue = 2115, Name = "Tome-Seeker's Blade" }); // Subtle silver hue
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
