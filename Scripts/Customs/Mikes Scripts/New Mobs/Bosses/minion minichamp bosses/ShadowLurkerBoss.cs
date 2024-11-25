using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the shadow overlord")]
    public class ShadowLurkerBoss : ShadowLurker
    {
        [Constructable]
        public ShadowLurkerBoss() : base()
        {
            Name = "Shadow Overlord";
            Title = "the Supreme Lurker";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(600, 900); // Enhanced strength
            SetDex(600, 900); // Enhanced dexterity
            SetInt(200, 350); // Enhanced intelligence

            SetHits(5000, 6000); // Higher health for boss-level challenge

            SetDamage(20, 40); // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 100); // Maxed poison resistance
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Stealing, 100.0, 120.0);
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);

            Fame = 12000; // Increased fame for a boss
            Karma = -12000; // Boss-level karma

            VirtualArmor = 60; // Enhanced virtual armor

            // Attach the XmlRandomAbility for dynamic combat
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
            // Additional boss-level behavior can be implemented here
        }

        public ShadowLurkerBoss(Serial serial) : base(serial)
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
