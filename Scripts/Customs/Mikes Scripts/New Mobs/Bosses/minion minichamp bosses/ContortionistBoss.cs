using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the contortionist overlord")]
    public class ContortionistBoss : Contortionist
    {
        [Constructable]
        public ContortionistBoss() : base()
        {
            Name = "Contortionist Overlord";
            Title = "the Untouchable";

            // Update stats to match or exceed Barracoon's (or as needed)
            SetStr(425); // Increase strength
            SetDex(600); // Increase dexterity
            SetInt(300); // Increase intelligence

            SetHits(12000); // Set health comparable to Barracoon
            SetDamage(29, 38); // Increase damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Increase resistance
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Increase skill range
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);

            Fame = 22500; // Set higher fame to reflect boss status
            Karma = -22500; // Maintain negative karma

            VirtualArmor = 70; // Increase virtual armor

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
			PackItem(new GlimmeringBloodrock());
			PackItem(new GreyWolfSummoningMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic (e.g., combat strategies) could be added here
        }

        public ContortionistBoss(Serial serial) : base(serial)
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
