using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class JuniorsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Junior’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Marek Bronzebeard*, the renowned Tinsmith of East Montor, polishing a half-finished helm. His hands are steady, but his eyes betray the weight of old regrets.\n\n" +
                    "He looks up, voice low but firm:\n\n" +
                    "“I made the helm, you know. For the young one. The Drakkon heir. Forged in silversteel, crowned in flame—it was to mark his rise.”\n\n" +
                    "“But something twisted him. Or maybe he was born wrong. Now he roams the **Caves of Drakkon**, roaring like a storm, hungry for power.”\n\n" +
                    "“If he takes his father’s crown, it won’t just be dragons we fear. It’ll be the skies themselves.”\n\n" +
                    "**Slay DrakkonHeirJr** before he claims dominion over fire and flight.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the mountain crumble under his roar. I won’t forge another helm for a monster.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still he lives? His roar grows louder by the night... It shakes the anvils.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain him? The roar is gone?...\n\n" +
                       "*Marek’s shoulders sink, relief mingled with sorrow.*\n\n" +
                       "“The crown remains unclaimed. The skies safe... for now.”\n\n" +
                       "Take this chest—old work, but sturdy. May it keep your treasures safe, as you’ve kept us.”";
            }
        }

        public JuniorsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakkonHeirJr), "DrakkonHeirJr", 1));
            AddReward(new BaseReward(typeof(MedievalEnglandChest), 1, "MedievalEnglandChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Junior’s End'!");
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

    public class MarekBronzebeard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(JuniorsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this)); // Closest vendor type for a Tinsmith
        }

        [Constructable]
        public MarekBronzebeard()
            : base("the Tinsmith", "Marek Bronzebeard")
        {
        }

        public MarekBronzebeard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 90, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered bronze hue
            HairItemID = 0x203C; // Long Hair
            HairHue = 2207; // Metallic bronze
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 2207;
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron() { Hue = 2419, Name = "Forgemaster's Apron" });
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Tinsmith's Mitts" });
            AddItem(new StuddedChest() { Hue = 2117, Name = "Bronzebeard's Vest" });
            AddItem(new LeatherLegs() { Hue = 1815, Name = "Cinder-Touched Leggings" });
            AddItem(new Boots() { Hue = 1811, Name = "Anvil-Stompers" });

            AddItem(new SmithSmasher() { Hue = 2420, Name = "Molten Maul" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Tinsmith’s Satchel";
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
