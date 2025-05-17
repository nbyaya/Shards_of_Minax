using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilentRoarQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Roar"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand at Moon's threshold, and I ask your aid.\n\n" +
                    "I am Mirith Dawnwatch, keeper of this city's gate. For weeks now, the Desert Grizzly has ambushed our supply wagons, " +
                    "its roar shattering our defenses as if they were twigs.\n\n" +
                    "**Hunt and slay the Desert Grizzly** that prowls the dunes near the Moon Gate. We cannot hold long without fresh supplies.";
            }
        }

        public override object Refuse { get { return "Then pray you never hear the grizzly's roar up close."; } }

        public override object Uncomplete { get { return "The beast still breathes. The wagons remain vulnerable."; } }

        public override object Complete { get { return "You silenced its roar? The gate stands because of you. Accept this token of our gratitude."; } }

        public SilentRoarQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DesertGrizzly), "Desert Grizzly", 1));

            AddReward(new BaseReward(typeof(WhisperspikeCollar), 1, "WhisperspikeCollar"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Roar'!");
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

    public class MirithDawnwatch : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilentRoarQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmer()); 
        }

        [Constructable]
        public MirithDawnwatch() : base("Mirith Dawnwatch", "Gatekeeper of Moon")
        {
            Title = "Gatekeeper of Moon";
			Body = 0x190; // Male human

            // Unique Outfit
            AddItem(new PlateDo { Hue = 1150, Name = "Starforged Breastplate" }); // Deep shimmering silver-blue
            AddItem(new StuddedLegs { Hue = 2401, Name = "Moonbound Greaves" }); // Pale steel, desert-weathered
            AddItem(new LeatherGloves { Hue = 2515, Name = "Dawnwatcher's Grips" }); // Muted gold
            AddItem(new WingedHelm { Hue = 1153, Name = "Helm of Celestial Vigil" }); // Blue-tinged silver, wing motifs
            AddItem(new Cloak { Hue = 1175, Name = "Nightwind Cloak" }); // Deep indigo with starry patterns
            AddItem(new Boots { Hue = 2406, Name = "Sandwalker Boots" }); // Desert-worn, dusty gray

            // Weapon & Flair
            AddItem(new CrescentBlade { Hue = 1157, Name = "Gatekeeper's Crescent" }); // Ornate crescent-shaped blade
            AddItem(new BodySash { Hue = 1170, Name = "Sash of the Outer Gate" }); // Light silver-blue sash

            SetStr(85, 95);
            SetDex(75, 85);
            SetInt(70, 80);

            SetDamage(6, 12);
            SetHits(250, 270);
        }

        public MirithDawnwatch(Serial serial) : base(serial) { }

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
