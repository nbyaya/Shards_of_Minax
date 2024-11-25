using System;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the corporate overlord")]
    public class CorporateExecutiveBoss : CorporateExecutive
    {
        [Constructable]
        public CorporateExecutiveBoss() : base()
        {
            Name = "Corporate Overlord";
            Title = "the Supreme Executive";

            // Update stats to match or exceed Barracoon-like levels
            SetStr(1200); // High strength for the boss
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence for powerful magic

            SetHits(12000); // High health for a boss
            SetDamage(29, 38); // Increased damage range for difficulty

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire, 15);
            SetDamageType(ResistanceType.Energy, 15);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;

            // Attach random ability for added randomness
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic could go here, like casting powerful spells or summoning stronger creatures
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            PackGold(5000, 10000); // Significant gold reward
        }

        public CorporateExecutiveBoss(Serial serial) : base(serial)
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
