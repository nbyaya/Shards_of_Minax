using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ExtinguishEmberKitsuneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Extinguish the EmberKitsune"; } }

        public override object Description
        {
            get
            {
                return
                    "Kaelin Moon, the Star Seer, gazes into the celestial globe that glows faintly in her hands.\n\n" +
                    "Her eyes are distant, reflecting a turmoil far beyond the skies.\n\n" +
                    "“The **EmberKitsune**... its flame disrupts the stars. I see it dancing across my charts, a spark that will ignite calamity if left unchecked.”\n\n" +
                    "“Long ago, I trapped a fox spirit under a lunar eclipse. This one is kin to it, or worse. It slipped past the Gate of Hell’s weakened seal.”\n\n" +
                    "“Slay the EmberKitsune, and restore balance. My visions are clouded while its fire rages.”\n\n" +
                    "**Track and slay the EmberKitsune** to soothe the skies and quell the fire that taints your fate.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the flames shall rise unchecked, and my charts will burn with futures best left unspoken.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Its fire still dances? I feel the heat in my dreams now, the stars bleed light.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The skies are quieter now, and the stars resume their gentle course.\n\n" +
                       "**You have soothed the ember, and stilled the flame.**\n\n" +
                       "Take this: the *TwoShotCrossbow*. Let it strike as swift and true as the stars themselves.";
            }
        }

        public ExtinguishEmberKitsuneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EmberKitsune), "EmberKitsune", 1));
            AddReward(new BaseReward(typeof(TwoShotCrossbow), 1, "TwoShotCrossbow"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Extinguish the EmberKitsune'!");
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

    public class KaelinMoon : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ExtinguishEmberKitsuneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller());
        }

        [Constructable]
        public KaelinMoon()
            : base("the Star Seer", "Kaelin Moon")
        {
        }

        public KaelinMoon(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Long Hair
            HairHue = 1153; // Silver-white
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2075, Name = "Starwoven Blouse" }); // Deep midnight blue
            AddItem(new FancyKilt() { Hue = 1153, Name = "Moonlit Kilt" }); // Silver-white
            AddItem(new Sandals() { Hue = 1157, Name = "Skybound Sandals" }); // Pale blue
            AddItem(new Cloak() { Hue = 1160, Name = "Veil of the Night Sky" }); // Dark indigo
            AddItem(new WizardsHat() { Hue = 2075, Name = "Ecliptic Crown" }); // Matches blouse

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Seer's Satchel";
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
