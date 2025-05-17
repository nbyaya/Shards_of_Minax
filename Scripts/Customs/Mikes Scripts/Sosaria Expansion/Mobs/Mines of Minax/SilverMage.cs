using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fifth;  // For Energy Vortex, etc.
using Server.Spells.Seventh; // For Chain Lightning
using Server.Spells.Sixth;   // For Paralyze

namespace Server.Mobiles
{
    [CorpseName("a silver mage corpse")]
    public class SilverMage : DragonsFlameGrandMage
    {
        // --- Timers for special abilities ---
        private DateTime m_NextMirrorTime;
        private DateTime m_NextSiphonTime;
        private DateTime m_NextShardTime;
        private Point3D m_LastLocation;

        // Unique silver hue
        private const int UniqueHue = 1158;

        [Constructable]
        public SilverMage() : base()
        {
            // Basic identity
            Name  = "a Silver Mage";
            Title = "Master of the Argent Circle";
            Hue   = UniqueHue;

            // Significantly beefed‑up stats
            SetStr(400, 440);
            SetDex(250, 280);
            SetInt(700, 750);

            SetHits(1800, 2100);
            SetStam(300, 350);
            SetMana(800, 900);

            // Damage and Resistances
            SetDamage(25, 35);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 75, 85);

            // Skills
            SetSkill(SkillName.EvalInt,     120.0, 130.0);
            SetSkill(SkillName.Magery,      120.0, 130.0);
            SetSkill(SkillName.MagicResist, 125.0, 140.0);
            SetSkill(SkillName.Meditation,  110.0, 120.0);
            SetSkill(SkillName.Tactics,      95.0, 105.0);
            SetSkill(SkillName.Wrestling,    95.0, 105.0);

            Fame          = 30000;
            Karma         = -30000;
            VirtualArmor  = 90;
            ControlSlots  = 6;

            // Initialize cooldowns
            m_NextMirrorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));

            m_LastLocation = this.Location;

            // Starter reagents & loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
        }

        // Reflective Barrier: briefly reflects melee damage back to attackers
        private bool m_BarrierActive = false;
        public void ActivateBarrier()
        {
            this.Say("*Behold the Argent Aegis!*");
            m_BarrierActive = true;

            Timer.DelayCall(TimeSpan.FromSeconds(8), () =>
            {
                m_BarrierActive = false;
                this.Say("*The barrier fades.*");
            });
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_BarrierActive && from != null && from.Alive && from.InRange(this, 4))
            {
                // Reflect 50% of the incoming damage
                int reflect = (int)(amount * 0.5);
                AOS.Damage(from, this, reflect, 0, 0, 0, 0, 100);
                from.SendMessage(0x35, "Your strike is repelled by a silvery shield!");
                from.PlaySound(0x1F8);
            }
        }

        // Drain an enemy’s mana in a cone
        public void ManaSiphon(Mobile target)
        {
            this.Say("*Yield your arcane essence!*");
            PlaySound(0x20E);

            List<Mobile> cone = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m)
                    && InLOS(m) && this.Direction == GetDirectionTo(m) )
                {
                    cone.Add(m);
                }
            }
            eable.Free();

            foreach (Mobile m in cone)
            {
                DoHarmful(m);
                if (m is Mobile mob)
                {
                    int drained = Utility.RandomMinMax(30, 50);
                    if (mob.Mana >= drained)
                    {
                        mob.Mana -= drained;
                        mob.SendMessage(0x22, "You feel your magic pulled into a silver vortex!");
                        mob.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    }
                    AOS.Damage(mob, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 0, 100);
                }
            }
        }

        // Rains silver shards around itself
        public void ShardStorm()
        {
            this.Say("*Shards of Argent!*");
            PlaySound(0x208);

            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                    m.FixedParticles(0x3789, 8, 20, 5032, UniqueHue, 0, EffectLayer.Waist);
                }
            }
            eable.Free();
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && Alive && Map != null && Map != Map.Internal)
            {
                // Mirror Barrier
                if (DateTime.UtcNow >= m_NextMirrorTime && InRange(target.Location, 12))
                {
                    ActivateBarrier();
                    m_NextMirrorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
                // Mana Siphon
                else if (DateTime.UtcNow >= m_NextSiphonTime && InRange(target.Location, 10))
                {
                    ManaSiphon(target);
                    m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
                }
                // Shard Storm
                else if (DateTime.UtcNow >= m_NextShardTime && InRange(target.Location, 8))
                {
                    ShardStorm();
                    m_NextShardTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
                }
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My power… shattered…*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 12, 60, UniqueHue, 0, 5052, 0);

                // Spawn random silver shard hazards
                int count = Utility.RandomMinMax(5, 9);
                for (int i = 0; i < count; i++)
                {
                    int x = X + Utility.RandomMinMax(-4, 4);
                    int y = Y + Utility.RandomMinMax(-4, 4);
                    int z = this.Z;
                    Point3D loc = new Point3D(x, y, z);

                    if (!Map.CanFit(x, y, z, 16, false, false))
                        loc.Z = Map.GetAverageZ(x, y);

                    IceShardTile shard = new IceShardTile();
                    shard.Hue = UniqueHue;
                    shard.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                        0x376A, 8, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 14));

            // 3% chance for a unique Silver Focus staff
            if (Utility.RandomDouble() < 0.03)
                PackItem(new HideOfTheForestKin());

            base.GenerateLoot();
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus    { get { return 80.0;   } }

        public SilverMage(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑initialize cooldowns after load
            m_NextMirrorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_LastLocation   = this.Location;
        }
    }
}
