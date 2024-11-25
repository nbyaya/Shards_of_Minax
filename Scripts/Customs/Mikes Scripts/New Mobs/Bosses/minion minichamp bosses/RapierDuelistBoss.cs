using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the rapier grandmaster")]
    public class RapierDuelistBoss : RapierDuelist
    {
        [Constructable]
        public RapierDuelistBoss() : base()
        {
            Name = "Rapier Grandmaster";
            Title = "the Unmatched Duelist";

            // Enhanced stats for a boss-level challenge
            SetStr(800, 1000); // Higher strength for more damage
            SetDex(300, 350); // Increased dexterity for better agility
            SetInt(150, 200); // Increased intelligence for more mana and resistances

            SetHits(1200, 1500); // Increased health for survivability

            SetDamage(20, 30); // Increased damage range to make it more dangerous

            SetResistance(ResistanceType.Physical, 70, 80); // Higher resistance values for survivability
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Fencing, 120.0, 140.0); // Increased fencing skill for better combat ability
            SetSkill(SkillName.Tactics, 110.0, 130.0); // Higher tactics for better attack strategies
            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Higher anatomy skill for more efficient damage
            SetSkill(SkillName.Parry, 100.0, 120.0); // Increased parry skill for better defense

            Fame = 10000; // Increased fame to match the boss-tier difficulty
            Karma = -10000; // Negative karma for a powerful boss

            VirtualArmor = 60; // Higher armor to improve defense

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
            // Additional boss-specific behavior can be added here if needed
        }

        public RapierDuelistBoss(Serial serial) : base(serial)
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
