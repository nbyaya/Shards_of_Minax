using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShadesInTheSmokeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shades in the Smoke"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself face-to-face with *Sonya “Smokes” Calavera*, the sharp-tongued, smoke-scarred keeper of Pirate Isle’s most infamous tavern.\n\n" +
                    "She adjusts her ash-streaked tricorn, eyes darting to the hearth behind her where flames flicker and die in defiance.\n\n" +
                    "“Aye, you feel it? The cold in the fire?” she mutters, knuckles white around a blackened tankard.\n\n" +
                    "“There’s a shade, born of ash and spite, haunts my cellar. Was just a spark once—came crawling from Exodus’ cursed halls. Now? It snuffs flame, it whispers… and I’ve no patience left for ghosts.”\n\n" +
                    "“Slay it, will ya? The **CinderedShade**. Do that, and I’ll give you somethin'... a banner from the plague years, cursed but proud.”\n\n" +
                    "**End the Shade. Bring back my fire.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then freeze with the rest. I’ll find another with steel and guts to burn it out.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it lingers? I hear the whispers louder now. The casks are growing cold...";
            }
        }

        public override object Complete
        {
            get
            {
                return "Gone? Truly?\n\n" +
                       "Bless you, firewalker. The hearth’s alive again. Take this, as promised—the *PlagueBanner*. Let it remind you: even ash bears history.";
            }
        }

        public ShadesInTheSmokeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CinderedShade), "CinderedShade", 1));
            AddReward(new BaseReward(typeof(PlagueBanner), 1, "PlagueBanner"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shades in the Smoke'!");
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

    public class SonyaCalavera : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShadesInTheSmokeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTavernKeeper());
        }

        [Constructable]
        public SonyaCalavera()
            : base("the Ashen Tavern Keeper", "Sonya 'Smokes' Calavera")
        {
        }

        public SonyaCalavera(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Pale ash-grey
            HairItemID = 0x2044; // Long Hair
            HairHue = 1109; // Smoke grey
        }

        public override void InitOutfit()
        {
            AddItem(new TricorneHat() { Hue = 1154, Name = "Scorched Tricorne" });
            AddItem(new FancyShirt() { Hue = 1150, Name = "Smoke-Streaked Shirt" });
            AddItem(new LeatherBustierArms() { Hue = 2407, Name = "Emberbound Vest" });
            AddItem(new LeatherSkirt() { Hue = 2101, Name = "Cinder-Weave Skirt" });
            AddItem(new ThighBoots() { Hue = 1109, Name = "Ash-Walker Boots" });
            AddItem(new HalfApron() { Hue = 1175, Name = "Tavernkeeper’s Apron" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Smoke-Cured Pack";
            AddItem(backpack);

            AddItem(new CooksCleaver() { Hue = 1153, Name = "Sooted Cleaver" });
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
