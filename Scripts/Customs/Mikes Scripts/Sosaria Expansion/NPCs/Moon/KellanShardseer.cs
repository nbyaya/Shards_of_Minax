using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WingedSentinelQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Winged Sentinel"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Kellan Shardseer*, an agile figure wrapped in shimmering fabrics, perched near the base of Moon's west tower.\n\n" +
                    "He peers skyward, one hand gripping a peculiar grappling hook, the other tracing a constellation on a faded scroll.\n\n" +
                    "“That blasted *Pyramid Crane*—a sentinel of wind and wing—has claimed the vaults atop this tower. It's more than a beast, traveler. It was placed there to guard what I seek: relics hidden by sky-priests long before Moon rose from the sands.”\n\n" +
                    "“I’ve climbed before, danced on ledges thinner than trust—but this one... it calls the wind, knocks even the sure-footed to their doom.”\n\n" +
                    "“Help me clear the roost. Kill the crane, and I can reach the treasures that sing to me from above. I’ll make it worth your while—with a gift drawn from the breath of the heavens themselves.”\n\n" +
                    "**Slay the Pyramid Crane** and return so I may climb where only stars have tread.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "The winds will take what they please, then. But if that crane still reigns, none will climb without tasting the sky’s wrath.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it perches? Then the vaults remain untouched. The winds mock me, friend.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The crane is no more? You’ve *tamed the winds*—or at least silenced them.\n\n" +
                       "With its wings stilled, I’ll ascend and claim what waits in shadowed roosts. As promised, take this: *The Ascendant Gale*—may it lift you as the wind once defied me.";
            }
        }

        public WingedSentinelQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PyramidCrane), "Pyramid Crane", 1));
            AddReward(new BaseReward(typeof(TheAscendantGale), 1, "The Ascendant Gale"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Winged Sentinel'!");
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

    public class KellanShardseer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WingedSentinelQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSATailor()); 
        }

        [Constructable]
        public KellanShardseer()
            : base("the Artifact Hunter", "Kellan Shardseer")
        {
        }

        public KellanShardseer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 50);

            Female = false;
            Body = 0x190; // Male Human
            Race = Race.Human;

            Hue = 1135; // Pale, wind-bitten skin
            HairItemID = 0x203B; // Wild short hair
            HairHue = 1153; // Midnight Blue
            FacialHairItemID = 0x2041; // Trim beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1109, Name = "Skyweave Tunic" }); // Silver-gray shimmer
            AddItem(new Hakama() { Hue = 2413, Name = "Vault-Seeker's Leggings" }); // Deep indigo
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Windshear Cloak" }); // Dark shimmer
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Climber's Grip" }); // Dusty beige
            AddItem(new Sandals() { Hue = 2418, Name = "Treader's Soles" }); // Desert gold

            AddItem(new Rope() { Hue = 0, Name = "Grappling Hook" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Artifact Satchel";
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
