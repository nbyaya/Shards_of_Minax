using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the vengeful druid overlord")]
    public class HostileDruidBoss : HostileDruid
    {
        [Constructable]
        public HostileDruidBoss() : base()
        {
            Name = "Vengeful Druid Overlord";
            Title = "the Forest's Wrath";

            // Enhance stats to be boss-tier (match or exceed HostileDruid stats)
            SetStr(1200); // Upper-end strength
            SetDex(255);  // Upper-end dexterity
            SetInt(250);  // Upper-end intelligence

            SetHits(10000); // High hit points for a boss-tier NPC

            SetDamage(20, 40); // Increase damage range for difficulty

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);  // Stronger magic resistance
            SetSkill(SkillName.Tactics, 120.0);  // Increase tactics for boss behavior
            SetSkill(SkillName.Wrestling, 120.0);  // Increase wrestling to match strength

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // More armor for higher resilience

            // Attach a random ability for dynamic behavior
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
            // Additional boss behavior or speech could be added here if necessary
        }

        public HostileDruidBoss(Serial serial) : base(serial)
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
