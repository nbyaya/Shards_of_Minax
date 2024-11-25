using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the toxicologist chef overlord")]
    public class ToxicologistChefBoss : ToxicologistChef
    {
        [Constructable]
        public ToxicologistChefBoss() : base()
        {
            Name = "Toxicologist Chef Overlord";
            Title = "the Supreme Chef";

            // Update stats to match or exceed Barracoon (similar approach to other bosses)
            SetStr(700, 1000); // Stronger strength, similar to Barracoon
            SetDex(150, 200); // Enhanced dexterity
            SetInt(400, 600); // Enhanced intelligence

            SetHits(12000); // Increase health to match Barracoon-like boss
            SetDamage(25, 40); // Enhanced damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 100.0, 120.0);
            SetSkill(SkillName.Cooking, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased negative karma

            VirtualArmor = 80; // Increased virtual armor

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

            // Additional dialogue on death
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "My recipes... will live on..."); break;
                case 1: this.Say(true, "You'll regret this..."); break;
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic can be added here, if needed
        }

        public ToxicologistChefBoss(Serial serial) : base(serial)
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
