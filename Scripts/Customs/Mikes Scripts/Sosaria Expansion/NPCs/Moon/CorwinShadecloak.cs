using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RottingMassQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Rotting Mass"; } }

        public override object Description
        {
            get
            {
                return
                    "The Moon Temple lies in peril...\n\n" +
                    "I am Corwin Shadecloak, Sentinel of these sacred halls. An abomination, a CursedCorpse, festers in the crypts below, " +
                    "born of the temple’s ancient necrotic aura. It consumes those who wander too near, their souls trapped in decay.\n\n" +
                    "**Destroy the Cursed Corpse** before it regenerates. Burn its fragments—only then will the temple know peace.";
            }
        }

        public override object Refuse { get { return "Then pray the darkness claims you not, stranger."; } }

        public override object Uncomplete { get { return "The CursedCorpse yet clings to its rotten flesh. You must strike it down completely."; } }

        public override object Complete { get { return "The crypt's foulness lifts, if only for a time. Take this—a relic worn by those who first defended this hold."; } }

        public RottingMassQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedCorpse), "CursedCorpse", 1));

            AddReward(new BaseReward(typeof(BannercladSkirtOfTheFirstHold), 1, "BannercladSkirtOfTheFirstHold"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Rotting Mass'!");
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

    public class CorwinShadecloak : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RottingMassQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage()); 
        }

        [Constructable]
        public CorwinShadecloak() : base("Corwin Shadecloak", "Temple Sentinel")
        {
            Title = "Temple Sentinel";
			Body = 0x190; // Male human

            // Outfit - Moon Temple aesthetic, shadowy but noble
            AddItem(new HoodedShroudOfShadows { Hue = 1109, Name = "Moonveil Shroud" }); // Dark grey, symbol of sentinel order
            AddItem(new LeatherGloves { Hue = 1150, Name = "Warden's Gauntlets" }); // Deep midnight blue
            AddItem(new LeatherLegs { Hue = 1150, Name = "Cryptwalker's Guards" }); // Same hue, for stealth
            AddItem(new Cloak { Hue = 1102, Name = "Shadebound Cloak" }); // Black with a subtle blue shimmer
            AddItem(new Boots { Hue = 1153, Name = "Silent Path Boots" }); // Dull silver-gray
            AddItem(new BodySash { Hue = 1150, Name = "Sentinel's Sash" }); // Matches gloves and legs
            AddItem(new BoneHarvester { Hue = 1150, Name = "Relicblade of Night Vigil" }); // Weapon tied to temple's defenders

            SetStr(85, 100);
            SetDex(80, 95);
            SetInt(90, 105);

            SetDamage(10, 15);
            SetHits(250, 270);
        }

        public CorwinShadecloak(Serial serial) : base(serial) { }

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
