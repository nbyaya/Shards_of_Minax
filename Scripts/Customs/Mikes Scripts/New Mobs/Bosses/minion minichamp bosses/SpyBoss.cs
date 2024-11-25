using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the spy overlord")]
    public class SpyBoss : Spy
    {
        [Constructable]
        public SpyBoss() : base()
        {
            Name = "Spy Overlord";
            Title = "the Master of Secrets";

            // Update stats to match or exceed Barracoon-like stats
            SetStr(425); // Adjusted strength for a tougher boss
            SetDex(150); // Adjusted dexterity for a more agile boss
            SetInt(750); // Adjusted intelligence to make the boss more powerful

            SetHits(12000); // Set health to match Barracoon's level
            SetStam(300); // Stamina to match Barracoon's levels
            SetMana(750); // Mana is set high for a powerful mage boss

            SetDamage(29, 38); // Damage range set to match Barracoon's

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Hiding, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            // Attach the random ability XML attachment
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
            // Additional boss-specific logic could go here, if necessary
        }

        public SpyBoss(Serial serial) : base(serial)
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
