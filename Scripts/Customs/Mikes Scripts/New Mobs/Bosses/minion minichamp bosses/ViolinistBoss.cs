using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the violin master")]
    public class ViolinistBoss : Violinist
    {
        [Constructable]
        public ViolinistBoss() : base()
        {
            Name = "Violin Master";
            Title = "the Supreme Violinist";

            // Update stats to match or exceed Barracoon
            SetStr(400, 500); // Slightly better than original
            SetDex(150, 180); // Higher dexterity than original
            SetInt(500, 600); // Higher intelligence than original

            SetHits(12000); // Much higher health for a boss-level entity
            SetStam(300); // Higher stamina
            SetMana(750); // Higher mana

            SetDamage(15, 30); // Stronger damage

            SetResistance(ResistanceType.Physical, 60, 75); // Higher resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 120.0); // Higher magic resistance
            SetSkill(SkillName.Tactics, 100.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 100.0); // Higher wrestling skill
            SetSkill(SkillName.Magery, 120.0); // Higher magery skill

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased negative karma for the boss

            VirtualArmor = 70; // Increased armor

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
            // Additional boss behavior could go here
        }

        public ViolinistBoss(Serial serial) : base(serial)
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
