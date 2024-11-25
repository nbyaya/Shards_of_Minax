using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the poison overlord")]
    public class HerbalistPoisonerBoss : HerbalistPoisoner
    {
        [Constructable]
        public HerbalistPoisonerBoss() : base()
        {
            Name = "Poison Overlord";
            Title = "the Supreme Herbalist Poisoner";

            // Update stats to match or exceed Barracoon
            SetStr(425); // Strength enhancement for boss
            SetDex(150); // Dexterity enhancement for boss
            SetInt(750); // Intelligence enhancement for boss

            SetHits(12000); // Increased health for boss tier
            SetStam(300); // Enhanced stamina for boss
            SetMana(750); // Enhanced mana for boss

            SetDamage(29, 38); // Enhanced damage to be more formidable

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Poisoning, 100.0); // Enhanced poisoning skill

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

        public HerbalistPoisonerBoss(Serial serial) : base(serial)
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
