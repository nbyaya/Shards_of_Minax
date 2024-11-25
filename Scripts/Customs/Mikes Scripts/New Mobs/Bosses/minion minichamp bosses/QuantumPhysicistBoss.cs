using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the quantum overlord")]
    public class QuantumPhysicistBoss : QuantumPhysicist
    {
        [Constructable]
        public QuantumPhysicistBoss() : base()
        {
            Name = "Quantum Overlord";
            Title = "the Supreme Physicist";

            // Update stats to match or exceed Barracoon (or similar)
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(150); // Matching Barracoon's upper dexterity
            SetInt(750); // Matching Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Matching Barracoon's upper stamina
            SetMana(750); // Matching Barracoon's upper mana

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

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
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public QuantumPhysicistBoss(Serial serial) : base(serial)
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
