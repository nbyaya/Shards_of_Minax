using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the kunoichi overlord")]
    public class KunoichiBoss : Kunoichi
    {
        [Constructable]
        public KunoichiBoss() : base()
        {
            Name = "Kunoichi Overlord";
            Title = "the Master Assassin";

            // Enhanced Stats to match or exceed Barracoon
            SetStr(600, 800); // Enhanced strength
            SetDex(300, 400); // Enhanced dexterity
            SetInt(150, 200); // Enhanced intelligence

            SetHits(10000, 12000); // Increased health to match boss tier

            SetDamage(35, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50); // Same as original but with stronger stats

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            // Enhanced skills
            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.Fencing, 110.1, 120.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 110.1, 120.0);
            SetSkill(SkillName.Ninjitsu, 100.1, 120.0);
            SetSkill(SkillName.Hiding, 100.1, 120.0);
            SetSkill(SkillName.Poisoning, 120.0); // Max skill in poisoning for higher threat

            Fame = 15000; // Increased fame to reflect boss tier
            Karma = -15000; // Negative karma for the villain nature

            VirtualArmor = 60; // Enhanced armor for increased defense

            // Attach random ability
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
            // Additional boss logic could be added here for more advanced behavior
        }

        public KunoichiBoss(Serial serial) : base(serial)
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
