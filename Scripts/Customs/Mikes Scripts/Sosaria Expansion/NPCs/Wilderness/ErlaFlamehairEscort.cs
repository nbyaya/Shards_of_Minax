using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ErlaFlamehairQuest : BaseQuest
    {
        public override object Title { get { return "Ashes and Emberlight"; } }

        public override object Description
        {
            get
            {
                return 
                    "*Her voice crackles like burning coal, fierce but frayed.*\n\n" +
                    "\"I'm Erla Flamehair, trapped here in Dardin’s Pit when the mine gods decided they'd test my mettle. I've clawed through fire and stone, but I can't make it to the surface alone—not with these gems in tow, and not with the ground still angry. Guide me out, and you’ll have earned more than thanks. You'll walk with the storm at your heels.\"";
            }
        }

        public override object Refuse { get { return "*She smirks grimly, brushing soot from her brow.* \"Fine. I'll dig my own grave if I have to.\""; } }
        public override object Uncomplete { get { return "*The heat intensifies as she glares at the path ahead.* \"The mine’s not done with us yet—move!\""; } }

        public ErlaFlamehairQuest() : base()
        {
            AddObjective(new EscortObjective("the Dardin's Pit Dungeon"));
            AddReward(new BaseReward(typeof(StormforgedLeggings), "StormforgedLeggings – Sturdy leggings resistant to fire and lightning."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Erla hands you the gleaming leggings, her eyes burning with fierce pride.* \"Take these. Fire and storm won’t lay you low with these on. I’m heading back to Devil Guard—but you, you’ve got storms to ride.\"");
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

    public class ErlaFlamehairEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(ErlaFlamehairQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public ErlaFlamehairEscort() : base()
        {
            Name = "Erla Flamehair";
            Title = "the Ember-Touched";
            NameHue = 0x66D; // Fiery red
        }

		public ErlaFlamehairEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 70, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 2413; // Tanned, soot-streaked skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1358; // Bright flame red
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 1354, Name = "Coal-Singed Vest" }); // Charred black with ember flickers
            AddItem(new StuddedArms() { Hue = 1161, Name = "Molten Leather Bracers" }); // Lava red
            AddItem(new LeatherGloves() { Hue = 1156, Name = "Ashen Grips" }); // Smoke-grey
            AddItem(new StuddedLegs() { Hue = 1153, Name = "Stormforged Leggings" }); // Dark steel, lightning etched
            AddItem(new ThighBoots() { Hue = 1175, Name = "Cindershade Boots" }); // Deep brown, heat-streaked
            AddItem(new Bandana() { Hue = 1359, Name = "Flamehair's Bind" }); // Glowing ember red
            AddItem(new Pickaxe() { Hue = 1109, Name = "Ironbiter" }); // Heavy, soot-streaked miner’s tool

            Backpack backpack = new Backpack();
            backpack.Hue = 1107;
            backpack.Name = "Gem-Laden Pack";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    string[] lines = new string[]
                    {
                        "*Erla wipes sweat from her brow.* These gems better be worth it... or I'll toss 'em to the lava myself.",
                        "*Her grip tightens on the pickaxe.* I’ve seen miners pulled into the stone... screaming all the way down.",
                        "*The heat shifts around her.* Dardin’s Pit always wants more blood. It won’t get mine.",
                        "*She spits into the dust.* Cursed place. Should’ve stayed in Devil Guard with the fools who dream of gold.",
                        "*A flicker of fear passes her eyes.* There’s something still moving down here... I can feel it.",
                        "*Erla glances back.* Keep your eyes sharp. Fire’s not the only thing that bites in these tunnels."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }
}
