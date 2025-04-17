using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class CustomOrc : BaseCreature
    {
        [Constructable]
        public CustomOrc()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a fierce orc";
            Body = 17;  // Orc body ID
            BaseSoundID = 0x45A;

            SetStr(96, 120);
            SetDex(81, 100);
            SetInt(36, 60);

            SetHits(58, 72);

            SetDamage(5, 10);

            SetSkill(SkillName.MagicResist, 25.1, 50.0);
            SetSkill(SkillName.Tactics, 29.3, 44.0);
            SetSkill(SkillName.Wrestling, 29.3, 44.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 18;

            // Add loot
            PackItem(new Gold(Utility.Random(10, 50))); // Gold loot
            AddScrollLoot(); // Adds the Scroll of Transcendence loot
        }

        public void AddScrollLoot()
        {
            if (Utility.RandomDouble() < 0.2) // 20% chance to drop a scroll
            {
                int minSkillPoints = 1; // Minimum skill increase (0.1 points)
                int maxSkillPoints = 10; // Maximum skill increase (1.0 points)

                ScrollOfTranscendence scroll = ScrollOfTranscendence.CreateRandom(minSkillPoints, maxSkillPoints);
                PackItem(scroll);
            }
        }

        public CustomOrc(Serial serial) : base(serial)
        {
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
