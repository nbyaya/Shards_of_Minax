using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the crying overlord")]
    public class CryingOrphanBoss : CryingOrphan
    {
        [Constructable]
        public CryingOrphanBoss() : base()
        {
            Name = "Crying Overlord";
            Title = "the Supreme Orphan";

            // Update stats to match or exceed Barracoon
            SetStr(425); // Enhanced strength to match higher tier boss standards
            SetDex(150); // Enhanced dexterity
            SetInt(750); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(20, 35); // Increased damage for boss tier

            SetResistance(ResistanceType.Physical, 75, 85); // Boss-tier resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);
            SetSkill(SkillName.EvalInt, 80.0, 100.0); // Added skill to enhance overall power

            Fame = 22500; // Increased fame for boss tier
            Karma = -22500; // Negative karma for the boss tier

            VirtualArmor = 70; // Boss-level virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new CaesarChest());
			PackItem(new RockNRallVault());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic or behaviors could be added here
        }

        public CryingOrphanBoss(Serial serial) : base(serial)
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
