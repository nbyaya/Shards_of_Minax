using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class PowerCrystalCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Power Crystal Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Alaric, the Keeper of the Crystal Vault. Long ago, a great calamity shattered the Crystal Vault, " +
                       "scattering the PowerCrystals across the land. These crystals are vital for the restoration of the vault and the safeguarding of ancient secrets. " +
                       "I require your aid to gather 50 PowerCrystals from the wilds. In exchange for your efforts, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and a unique Crystal Keeper's Attire that will mark your heroic deeds.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the PowerCrystals."; } }

        public override object Uncomplete { get { return "I still need 50 PowerCrystals. Bring them to me so I can continue the restoration of the Crystal Vault!"; } }

        public override object Complete { get { return "Splendid work! You have collected the 50 PowerCrystals I needed. Your bravery and dedication are truly commendable. " +
                       "Please accept these rewards as a token of my gratitude. May your future quests be as rewarding as this one!"; } }

        public PowerCrystalCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(PowerCrystal), "PowerCrystals", 50, 0x1F1C)); // Assuming PowerCrystal item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GadgeteersGauntlets), 1, "Crystal Keeper's Attire")); // Assuming Crystal Keeper's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Power Crystal Collector quest!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class CrystalKeeperAlaric : MondainQuester
    {
        [Constructable]
        public CrystalKeeperAlaric()
            : base("The Keeper of the Crystal Vault", "Alaric")
        {
        }

        public CrystalKeeperAlaric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Alaric's Crystal Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Alaric's Crystal Crown" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Alaric's Enchanted Cloak" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Alaric's Crystal Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(PowerCrystalCollectorQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
