using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of Chronos the Time Lord")]
    public class ChronosTheTimeLord : BaseCreature
    {
        private DateTime m_NextSpecialAbility;
        private bool m_IsEnraged;
        private Timer m_EnrageTimer;
        private Hashtable m_BleedEffects = new Hashtable();

        [Constructable]
        public ChronosTheTimeLord() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Chronos the Time Lord";
            Body = 400; // Liche
            Hue = 1000; // Ethereal blue

            SetStr(500);
            SetDex(150);
            SetInt(1000);

            SetHits(20000);
            SetMana(5000);
            SetStam(200);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 60;

            m_NextSpecialAbility = DateTime.UtcNow;
        }

        public ChronosTheTimeLord(Serial serial) : base(serial) { }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (DateTime.UtcNow >= m_NextSpecialAbility)
            {
                UseSpecialAbility();
                m_NextSpecialAbility = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }

            // Enrage at 30% health
            if (!m_IsEnraged && Hits < HitsMax * 0.3)
            {
                Enrage();
            }
        }

        private void UseSpecialAbility()
        {
            switch (Utility.Random(6))
            {
                case 0:
                    CreateGravityWell();
                    break;
                case 1:
                    Shapeshift();
                    break;
                case 2:
                    TimeSlow();
                    break;
                case 3:
                    ManaDrain();
                    break;
                case 4:
                    StaminaDrain();
                    break;
                case 5:
                    Bleed();
                    break;
            }
        }

        private void CreateGravityWell()
        {
            Say("Behold the power of gravity!");
            Point3D loc = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
            Item gravityWell = new GravityWellItem(TimeSpan.FromSeconds(10));
            gravityWell.MoveToWorld(loc, Map);
        }

        private void Shapeshift()
        {
            Say("Witness my true form!");
            switch (Body)
            {
                case 400: // Liche
                    Body = 9; // Gargoyle
                    break;
                case 9: // Gargoyle
                    Body = 747; // Ethereal Warrior
                    break;
                default:
                    Body = 400; // Back to Liche
                    break;
            }
            PlaySound(0x511);
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
        }

        private void TimeSlow()
        {
            Say("Time bends to my will!");
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    m.SendLocalizedMessage(500797); // You feel yourself moving in slow motion.
                    m.Paralyze(TimeSpan.FromSeconds(5));
                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Paralyze, 1075826, TimeSpan.FromSeconds(5), m, "Paralyzed"));
                }
            }
        }

        private void ManaDrain()
        {
            Say("Your magical essence feeds me!");
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    int drain = Utility.RandomMinMax(50, 100);
                    m.Mana -= drain;
                    Mana += drain;
                    m.SendLocalizedMessage(1070844); // Your mana has been drained!
                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                }
            }
        }

        private void StaminaDrain()
        {
            Say("Feel your energy fade away!");
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    int drain = Utility.RandomMinMax(50, 100);
                    m.Stam -= drain;
                    Stam += drain;
                    m.SendLocalizedMessage(1070776); // You feel your stamina drain away.
                    m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                }
            }
        }

        private void Bleed()
        {
            Say("Bleed for me!");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && CanBeHarmful(m))
                {
                    if (!m_BleedEffects.ContainsKey(m))
                    {
                        Timer t = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), () => DoBleed(m));
                        m_BleedEffects[m] = t;
                        m.SendLocalizedMessage(1060160); // You are bleeding!
                    }
                }
            }
        }

        private void DoBleed(Mobile m)
        {
            if (m.Alive)
            {
                AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 100, 0, 0, 0, 0);
                m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
            }
            else
            {
                if (m_BleedEffects.ContainsKey(m))
                {
                    ((Timer)m_BleedEffects[m]).Stop();
                    m_BleedEffects.Remove(m);
                }
            }
        }

        private void Enrage()
        {
            m_IsEnraged = true;
            Say("You've pushed me too far! Feel my wrath!");
            SetDamage(DamageMin * 2, DamageMax * 2);
            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            PlaySound(0x2F7);

            m_EnrageTimer = Timer.DelayCall(TimeSpan.FromMinutes(2), new TimerCallback(OnEnrageEnd));
        }

        private void OnEnrageEnd()
        {
            m_IsEnraged = false;
            SetDamage(DamageMin / 2, DamageMax / 2);
            Say("My power wanes... for now.");
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            
            foreach (Timer t in m_BleedEffects.Values)
            {
                t.Stop();
            }
            m_BleedEffects.Clear();

            if (m_EnrageTimer != null)
            {
                m_EnrageTimer.Stop();
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 3);
            AddLoot(LootPack.HighScrolls, 5);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_IsEnraged);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_IsEnraged = reader.ReadBool();
        }
    }

    public class GravityWellItem : Item
    {
        private Timer m_Timer;

        public GravityWellItem(TimeSpan duration) : base(0x1822) // Vortex appearance
        {
            Movable = false;
            Hue = 1194; // Dark purple
            Name = "Gravity Well";

            m_Timer = Timer.DelayCall(duration, Delete);
            Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), PullNearbyMobiles);
        }

        public GravityWellItem(Serial serial) : base(serial) { }

        private void PullNearbyMobiles()
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m is PlayerMobile)
                {
                    Point3D playerLoc = m.Location;
                    Point3D newLoc = new Point3D();

                    if (playerLoc.X < X)
                        newLoc.X = playerLoc.X + 1;
                    else if (playerLoc.X > X)
                        newLoc.X = playerLoc.X - 1;
                    else
                        newLoc.X = playerLoc.X;

                    if (playerLoc.Y < Y)
                        newLoc.Y = playerLoc.Y + 1;
                    else if (playerLoc.Y > Y)
                        newLoc.Y = playerLoc.Y - 1;
                    else
                        newLoc.Y = playerLoc.Y;

                    newLoc.Z = playerLoc.Z;

                    m.MoveToWorld(newLoc, Map);
                    m.SendLocalizedMessage(1080034); // You are pulled towards the gravity well!
                }
            }
        }

        public override void Delete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            base.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Delete(); // Delete on server load to prevent orphaned gravity wells
        }
    }
}
