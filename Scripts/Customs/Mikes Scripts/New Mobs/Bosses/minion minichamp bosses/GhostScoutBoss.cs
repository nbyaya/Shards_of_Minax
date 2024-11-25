using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ghost commander")]
    public class GhostScoutBoss : GhostScout
    {
        [Constructable]
        public GhostScoutBoss() : base()
        {
            Name = "Ghost Commander";
            Title = "the Wraith of the Shadows";

            // Enhanced stats to match or exceed Barracoon-like strength
            SetStr( 200, 250 ); // Higher strength
            SetDex( 300, 350 ); // Higher dexterity
            SetInt( 250, 300 ); // Higher intelligence

            SetHits( 3000, 4000 ); // Higher health

            SetDamage( 15, 25 ); // Increased damage range

            SetResistance( ResistanceType.Physical, 50, 60 ); // Stronger resistance
            SetResistance( ResistanceType.Fire, 30, 40 );
            SetResistance( ResistanceType.Cold, 70, 80 ); // Higher cold resistance
            SetResistance( ResistanceType.Poison, 40, 50 );
            SetResistance( ResistanceType.Energy, 60, 70 );

            SetSkill( SkillName.Hiding, 120.0 ); // Enhanced stealth skills
            SetSkill( SkillName.Stealth, 120.0 ); 
            SetSkill( SkillName.Anatomy, 80.0, 100.0 ); // Enhanced anatomy skill
            SetSkill( SkillName.Tactics, 100.0, 120.0 ); // Stronger tactics
            SetSkill( SkillName.Wrestling, 100.0, 120.0 ); // Enhanced wrestling skill

            Fame = 10000; // Higher fame
            Karma = -10000; // Higher karma penalty

            VirtualArmor = 60; // Higher virtual armor

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

            // Additional death phrases to give a more epic feel
            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My duty... is finished..."); break;
                case 1: this.Say(true, "The shadows... reclaim me..."); break;
            }
        }

        public GhostScoutBoss(Serial serial) : base(serial)
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
