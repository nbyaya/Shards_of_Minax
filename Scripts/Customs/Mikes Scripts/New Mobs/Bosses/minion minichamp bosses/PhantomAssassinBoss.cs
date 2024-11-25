using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the phantom assassin overlord")]
    public class PhantomAssassinBoss : PhantomAssassin
    {
        [Constructable]
        public PhantomAssassinBoss() : base()
        {
            Name = "Phantom Assassin Overlord";
            Title = "the Silent Death";

            // Update stats to make the boss stronger
            SetStr(700, 900); // Higher strength
            SetDex(300, 400); // Higher dexterity
            SetInt(200, 300); // Higher intelligence

            SetHits(12000); // Higher health
            SetDamage(40, 60); // Higher damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 50, 60); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 50, 60); // Higher cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Higher energy resistance

            SetSkill(SkillName.Anatomy, 120.0); // Higher anatomy skill
            SetSkill(SkillName.EvalInt, 120.0); // Higher eval int skill
            SetSkill(SkillName.Magery, 120.0); // Higher magery skill
            SetSkill(SkillName.Meditation, 100.0); // Higher meditation skill
            SetSkill(SkillName.MagicResist, 120.0); // Higher magic resist skill
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill

            Fame = 10000; // Higher fame
            Karma = -10000; // Higher negative karma

            VirtualArmor = 60; // Higher virtual armor

            // Attach the XmlRandomAbility to give the boss special abilities
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

            // Additional special drops (optional)
            this.Say(true, "The shadows shall claim you...");
        }

        public PhantomAssassinBoss(Serial serial) : base(serial)
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
