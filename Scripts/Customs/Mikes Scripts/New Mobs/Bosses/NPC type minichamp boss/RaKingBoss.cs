using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of RaKing the Overlord")]
    public class RaKingBoss : RaKing
    {
        [Constructable]
        public RaKingBoss() : base()
        {
            Name = "RaKing the Overlord";
            Title = "the Supreme King of Shadows";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Maximized strength
            SetDex(255); // Maximized dexterity
            SetInt(250); // Maximized intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(29, 38); // Higher damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Maxed resist skill
            SetSkill(SkillName.Tactics, 120.0); // Maxed tactics
            SetSkill(SkillName.Wrestling, 120.0); // Maxed wrestling
            SetSkill(SkillName.Magery, 100.0); // Strong magic skill

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma

            VirtualArmor = 70; // Enhanced armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Enhance loot further
            PackGold(500, 1000); // Richer gold drops
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss behavior or special tactics can go here
            if (Combatant != null && Utility.RandomDouble() < 0.1) // Random special ability or attack
            {
                Say("The shadows obey me!");
                // Implement further boss abilities or effects as needed
            }
        }

        public RaKingBoss(Serial serial) : base(serial)
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
