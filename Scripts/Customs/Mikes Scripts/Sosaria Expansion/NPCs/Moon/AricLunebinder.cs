using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PiousDisruptionsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Pious Disruptions"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Aric Lunebinder*, Curator of Moon’s Celestial Spire.\n\n" +
                    "“The stars speak of imbalance. A vile creature, the *AnointedBonePriest*, desecrates the sacred grounds beneath our moonlit skies.\n\n" +
                    "Its rituals warp the lunar blessings—our harmony is at risk. The stars tremble with each incantation. You must act.”\n\n" +
                    "**Slay the AnointedBonePriest** and restore balance to Moon’s sacred grounds.";
            }
        }

        public override object Refuse { get { return "Then may the stars shield us from what comes. I fear time is not on our side."; } }

        public override object Uncomplete { get { return "The Bone Priest still lives? The stars grow restless—find it, and end its rites."; } }

        public override object Complete { get { return "Balance is restored... for now. The heavens thank you, and so do I. Take this—a gift layered with vigilance."; } }

        public PiousDisruptionsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AnointedBonePriest), "AnointedBonePriest", 1));

            AddReward(new BaseReward(typeof(VigilantLayering), 1, "VigilantLayering"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Pious Disruptions'!");
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

    public class AricLunebinder : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PiousDisruptionsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker()); 
        }

        [Constructable]
        public AricLunebinder()
        {
            Name = "Aric Lunebinder";
            Title = "the Observatory Curator";
            Race = Race.Human;
            Body = 0x190; // Male human
            Hue = 0x83EA; // Subtle blue-grey tone, moon-touched.

            // Unique Outfit
            AddItem(new Robe() { Hue = 0x48D, Name = "Celestial Robe" }); // Deep cosmic blue
            AddItem(new WizardsHat() { Hue = 0x481, Name = "Lunebinder's Hat" }); // Pale silver
            AddItem(new Sandals() { Hue = 0x455, Name = "Star-Touched Sandals" }); // Soft grey
            AddItem(new GnarledStaff() { Hue = 0x47E, Name = "Staff of Stellar Wards" }); // Glowing wood
            AddItem(new Cloak() { Hue = 0x497, Name = "Mantle of the Voidwatcher" }); // Midnight hue

            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.Magery, 70.0, 90.0);
            SetSkill(SkillName.Inscribe, 60.0, 80.0);
        }

        public AricLunebinder(Serial serial) : base(serial) { }


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
