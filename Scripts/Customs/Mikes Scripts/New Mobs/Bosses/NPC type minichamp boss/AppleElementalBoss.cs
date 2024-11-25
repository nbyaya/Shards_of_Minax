using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    public class AppleElementalBoss : AppleElemental
    {
        [Constructable]
        public AppleElementalBoss() : base()
        {
            Name = "Apple Overlord";
            Title = "the Supreme Elemental";

            // Update stats to match or exceed the original
            SetStr(425);  // Higher strength for the boss
            SetDex(200);  // Keep dexterity the same, as it's already high
            SetInt(250);  // Increase intelligence to make the boss more challenging

            SetHits(3000); // Significantly higher health than the original

            SetDamage(50, 75); // Increased damage range

            // Update skills to be higher for a more challenging boss
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500; // Increased fame
            Karma = -22500; // Negative karma for a boss-tier entity

            VirtualArmor = 70; // Higher virtual armor for more defense

            // Attach a random ability for added unpredictability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            int applesToDrop = 20; // More apples for the boss-tier version

            for (int i = 0; i < applesToDrop; i++)
            {
                Point3D appleLocation = new Point3D(this.X + Utility.RandomMinMax(-2, 2), this.Y + Utility.RandomMinMax(-2, 2), this.Z);

                // Spawn the apple
                Apple droppedApple = new Apple();
                droppedApple.MoveToWorld(appleLocation, this.Map);

                // Send flamestrike effect at the apple's location
                Effects.SendLocationParticles(EffectItem.Create(appleLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 0, 0, 2023, 0);
            }
        }

        public AppleElementalBoss(Serial serial) : base(serial)
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
