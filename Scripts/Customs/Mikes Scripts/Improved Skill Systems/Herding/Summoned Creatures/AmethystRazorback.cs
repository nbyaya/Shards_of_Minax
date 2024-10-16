using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an amethyst razorback corpse")]
    public class AmethystRazorback : BaseCreature
    {
        private DateTime m_NextTuskCharge;

        [Constructable]
        public AmethystRazorback()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an amethyst razorback";
            Body = 0x122;
            BaseSoundID = 0xC4;
            Hue = 1373; // Vibrant purple hue

            SetStr(150, 200);
            SetDex(80, 100);
            SetInt(30, 50);

            SetHits(200, 250);
            SetMana(20, 40);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 65.0, 80.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.Wrestling, 80.0, 95.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextTuskCharge = DateTime.UtcNow;
        }

        public AmethystRazorback(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 4; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextTuskCharge)
            {
                DoTuskCharge();
            }
        }

        private void DoTuskCharge()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive && InRange(target.Location, 10))
            {
                Direction = GetDirectionTo(target);
                
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Amethyst Razorback prepares to charge! *");
                PlaySound(0x3E4); // Boar snort sound

                Timer.DelayCall(TimeSpan.FromSeconds(1.5), new TimerStateCallback(PerformCharge), target);

                m_NextTuskCharge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void PerformCharge(object state)
        {
            Mobile target = state as Mobile;
            if (target != null && target.Alive)
            {
                PlaySound(0x4A4); // Crashing sound
                this.Move(Direction);

                if (InRange(target.Location, 1))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                    target.Frozen = true;
                    target.SendLocalizedMessage(1070849); // The creature charges at you, slamming you with its tusks and knocking you to the ground!

                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerStateCallback(ReleaseTarget), target);
                }
            }
        }

        private void ReleaseTarget(object state)
        {
            Mobile target = state as Mobile;
            if (target != null)
            {
                target.Frozen = false;
                target.SendLocalizedMessage(1070849); // You recover from the impact and stand up.
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextTuskCharge = DateTime.UtcNow;
        }
    }
}