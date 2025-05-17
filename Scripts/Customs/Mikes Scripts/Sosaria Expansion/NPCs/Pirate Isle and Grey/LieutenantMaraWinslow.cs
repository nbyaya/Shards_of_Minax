using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SentryOfShadowsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Sentry of Shadows"; } }

        public override object Description
        {
            get
            {
                return
                    "Lieutenant *Mara Winslow* stands firm, arms crossed, eyes sharp beneath the tricorn’s brim. Her voice cuts through the sea air like steel:\n\n" +
                    "“The watch posts went silent... one by one. My men? Gone. Their horns? Shattered.”\n\n" +
                    "“We traced it—a **SpectralSentry**, born of old Exodus magics. It crosses lines unseen, snuffing signals and breaking the will of any who would resist.”\n\n" +
                    "“Each hour, I sound these beacon horns—hoping the sound will ward it off. But sound alone won’t kill shadows.”\n\n" +
                    "**Slay the SpectralSentry** that haunts the frontier and silences our defense.”";
            }
        }

        public override object Refuse
        {
            get { return "Then keep to the shadows yourself—but remember, it won't stop until no one sounds the horn."; }
        }

        public override object Uncomplete
        {
            get { return "The horns ring, but the Sentry still comes. More posts darken by the day."; }
        }

        public override object Complete
        {
            get
            {
                return "The horns sounded clear last night... no shadows in sight.\n\n" +
                       "You’ve done it. You’ve given us back our signal, our warning, our chance to stand.\n\n" +
                       "Take this: the **WabbajackClub**. May it strike confusion in your foes, as you’ve driven terror from ours.";
            }
        }

        public SentryOfShadowsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpectralSentry), "SpectralSentry", 1));
            AddReward(new BaseReward(typeof(WabbajackClub), 1, "WabbajackClub"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Sentry of Shadows'!");
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

    public class LieutenantMaraWinslow : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SentryOfShadowsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon()); // She’s a battle-trained garrison officer, so sword vendor fits.
        }

        [Constructable]
        public LieutenantMaraWinslow()
            : base("the Garrison Officer", "Lieutenant Mara Winslow")
        {
        }

        public LieutenantMaraWinslow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 75);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1044; // Weathered sea-worn skin tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new TricorneHat() { Hue = 1109, Name = "Windswept Tricorne" }); // Storm-grey
            AddItem(new StuddedDo() { Hue = 1157, Name = "Garrison Officer’s Brigandine" }); // Navy blue armor
            AddItem(new LeatherLegs() { Hue = 1157, Name = "Mariner's Greaves" });
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Signal-Caller's Gloves" });
            AddItem(new Cloak() { Hue = 1154, Name = "The Watcher's Cloak" }); // Seafoam hue
            AddItem(new Boots() { Hue = 1175, Name = "Night-Walker Boots" }); // Darkest onyx
            AddItem(new Cutlass() { Hue = 1153, Name = "Hornbreaker Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Signal Pack";
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
