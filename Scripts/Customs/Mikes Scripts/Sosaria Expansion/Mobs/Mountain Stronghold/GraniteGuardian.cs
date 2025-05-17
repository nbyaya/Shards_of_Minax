using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a shattered granite guardian corpse")]
    public class GraniteGuardian : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextStompTime;
        private DateTime m_NextBoulderTime;
        private DateTime m_NextMagmaTime;

        // Passive aura tracking
        private Point3D m_LastLocation;

        // Unique stone‑gray hue
        private const int UniqueHue = 2217;

        [Constructable]
        public GraniteGuardian()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a granite guardian";
            Body = 722;                       // same body as the undead guardian
            Hue = UniqueHue;
            // base sounds inherited; override specific sounds below
            // BaseSoundID left default

            // ---- Stats ----
            SetStr(350, 450);
            SetDex(100, 150);
            SetInt(80, 120);

            SetHits(1200, 1500);
            SetStam(200, 250);

            SetDamage(20, 30);

            // ---- Damage Types ----
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 10);
            SetDamageType(ResistanceType.Cold, 10);

            // ---- Resistances ----
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // ---- Skills ----
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics,    100.0, 110.0);
            SetSkill(SkillName.Wrestling,  110.0, 120.0);
            SetSkill(SkillName.Anatomy,     90.0, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 3;

            // ---- Ability Cooldowns ----
            m_NextStompTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagmaTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // ---- Loot ----
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new HuntingQueensTrail()); // unique drop placeholder
        }

        // ---- Passive Stone Aura: slows anyone who steps too close ----
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && m.InRange(this.Location, 1) && m.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile targetMobile)
                {
                    DoHarmful(targetMobile);
                    int stamDrain = Utility.RandomMinMax(5, 10);
                    if (targetMobile.Stam >= stamDrain)
                    {
                        targetMobile.Stam -= stamDrain;
                        targetMobile.SendLocalizedMessage(1060742); // "Your legs feel heavy as stone."
                        targetMobile.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Head);
                        targetMobile.PlaySound(0x208);
                    }
                }
            }
        }

        // ---- Special Attacks Dispatcher ----
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Prioritize based on proximity and cooldowns
            if (now >= m_NextStompTime && this.InRange(Combatant.Location, 2))
            {
                EarthenStompAttack();
                m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextMagmaTime && this.InRange(Combatant.Location, 12))
            {
                MagmaCrackAttack();
                m_NextMagmaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextBoulderTime && this.InRange(Combatant.Location, 10))
            {
                BoulderBarrageAttack();
                m_NextBoulderTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // ---- Earthen Stomp: 3‑tile AoE physical burst + quake tile ----
        public void EarthenStompAttack()
        {
            PlaySound(0x1FE);
            FixedParticles(0x36D4, 10, 20, 5042, UniqueHue, 0, EffectLayer.CenterFeet);

            // Spawn a central quake hazard
            var quake = new EarthquakeTile();
            quake.Hue = UniqueHue;
            quake.MoveToWorld(this.Location, this.Map);

            List<Mobile> victims = new List<Mobile>();
            var e = Map.GetMobilesInRange(this.Location, 3);
            foreach (Mobile m in e)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    victims.Add(m);
            }
            e.Free();

            foreach (var v in victims)
            {
                DoHarmful(v);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(v, this, dmg, 100, 0, 0, 0, 0);
            }
        }

        // ---- Magma Crack: targeted HotLavaTile under the combatant ----
        public void MagmaCrackAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*Molten fissure!*");
            PlaySound(0x20A);

            var loc = target.Location;
            Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 8, 30, UniqueHue, 0, 5032, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null) return;

                Point3D spawn = loc;
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(spawn, this.Map);

                Effects.PlaySound(spawn, this.Map, 0x21A);
            });
        }

        // ---- Boulder Barrage: bouncing physical projectiles ----
        public void BoulderBarrageAttack()
        {
            if (!(Combatant is Mobile initial)) return;

            Say("*Rocks take flight!*");
            PlaySound(0x20B);

            var targets = new List<Mobile> { initial };
            int maxBounces = 4, range = 6;

            // Find up to maxBounces additional targets
            for (int i = 0; i < maxBounces; i++)
            {
                var last = targets[targets.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                var e = Map.GetMobilesInRange(last.Location, range);
                foreach (Mobile m in e)
                {
                    if (m != this && m != last && !targets.Contains(m) &&
                        CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(last, m) &&
                        last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }
                e.Free();
                if (next == null) break;
                targets.Add(next);
            }

            // Launch boulders in sequence
            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36CE, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        int dmg = Utility.RandomMinMax(25, 35);
                        AOS.Damage(dst, this, dmg, 100, 0, 0, 0, 0);
                        dst.FixedParticles(0x36E4, 5, 15, 5032, EffectLayer.Waist);
                    }
                });
            }
        }

        // ---- Death Explosion: showers landmines around corpse ----
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*Guardian crumbles!*");
                Effects.PlaySound(Location, Map, 0x20D);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x3709, 10, 40, UniqueHue, 0, 5032, 0);

                int count = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < count; i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;
                    var loc = new Point3D(x, y, z);

                    if (!Map.CanFit(x, y, z, 16, false, false))
                        loc.Z = Map.GetAverageZ(x, y);

                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                                                  0x376A, 5, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // ---- Standard Properties & Loot Overrides ----
        public override bool BleedImmune  => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            // Already added in ctor; but can add scrolls or rare stones here
        }

        // ---- Sound Overrides ----
        public override int GetIdleSound()  => 1609;
        public override int GetAngerSound() => 1606;
        public override int GetHurtSound()  => 1608;
        public override int GetDeathSound() => 1607;

        // ---- Serialization ----
        public GraniteGuardian(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            m_NextStompTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagmaTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;
        }
    }
}
