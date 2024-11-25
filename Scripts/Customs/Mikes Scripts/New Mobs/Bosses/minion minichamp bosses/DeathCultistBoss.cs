using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the death cult leader")]
    public class DeathCultistBoss : DeathCultist
    {
        [Constructable]
        public DeathCultistBoss() : base()
        {
            Name = "Death Cult Leader";
            Title = "the Supreme Cultist";

            // Update stats to match or exceed Barracoon-like values
            SetStr(700, 1000); // Increase strength
            SetDex(150, 200); // Increase dexterity
            SetInt(400, 600); // Increase intelligence

            SetHits(8000, 10000); // Boss-level health

            SetDamage(25, 35); // Higher damage

            SetResistance(ResistanceType.Physical, 60, 75); // Increase resistances
            SetResistance(ResistanceType.Fire, 60, 75);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 15000; // Increased fame
            Karma = -15000; // Increased karma (boss)
            VirtualArmor = 75; // Boss-level armor

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
			PackItem(new TamersBindings());
			PackItem(new MonksMeditativeRobe());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, such as custom speech or effects
        }

        public DeathCultistBoss(Serial serial) : base(serial)
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
