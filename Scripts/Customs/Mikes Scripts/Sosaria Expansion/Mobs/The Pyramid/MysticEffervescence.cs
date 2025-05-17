using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // for Chain Lightning
using Server.Spells.Eighth;  // for higher‐tier effects

namespace Server.Mobiles
{
    [CorpseName("a mystic effervescence corpse")]
    public class MysticEffervescence : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextSurgeTime;
        private DateTime m_NextBubbleTime;
        private DateTime m_NextCascadeTime;
        private Point3D m_LastLocation;

        // A shimmering teal hue for its bubbles
        private const int UniqueHue = 1174;

        [Constructable]
        public MysticEffervescence()
            : base(AIType.AI_Spellweaving, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a mystic effervescence";
            Body = 0x111;
            BaseSoundID = 0x56E;
            Hue = UniqueHue;

            // ——— Stats ———
            SetStr(350, 400);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1200, 1500);
            SetStam(200, 250);
            SetMana(700, 850);

            SetDamage(20, 25);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 90, 100);

            // ——— Skills ———
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Tactics,   90.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Magery,    110.0, 125.0);
            SetSkill(SkillName.EvalInt,   110.0, 125.0);
            SetSkill(SkillName.Meditation,100.0, 110.0);
            SetSkill(SkillName.Spellweaving,100.0,115.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize ability timers
            m_NextSurgeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBubbleTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));

            m_LastLocation = this.Location;

            // Starter reagents
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
        }

        // ——— Aura: Effervescent Drain ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || m == this || !m.Alive || m.Map != this.Map || !InRange(m, 3))
                return;

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Drain stamina
                int stamDrain = Utility.RandomMinMax(10, 20);
                if (target.Stam >= stamDrain)
                {
                    target.Stam -= stamDrain;
                    target.FixedParticles(0x375A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                    target.PlaySound(0x1F9);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // ——— Bubble Trail ———
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var oldLoc = m_LastLocation;
                m_LastLocation = Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    var bubble = new VortexTile(); // swirling bubble‐like hazard
                    bubble.Hue = UniqueHue;
                    bubble.MoveToWorld(oldLoc, Map);
                }
            }
            else
            {
                m_LastLocation = Location;
            }

            // ——— Abilities ———
            if (DateTime.UtcNow >= m_NextSurgeTime && InRange(Combatant.Location, 8))
            {
                EffervescentSurge();
                m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            if (DateTime.UtcNow >= m_NextBubbleTime)
            {
                BubbleRift();
                m_NextBubbleTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            if (DateTime.UtcNow >= m_NextCascadeTime && InRange(Combatant.Location, 12))
            {
                ArcaneCascade();
                m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // ——— Effervescent Surge: AoE energy burst + stun chance ———
        private void EffervescentSurge()
        {
            PlaySound(0x212);
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var targets = new List<Mobile>();
            foreach (var o in Map.GetMobilesInRange(Location, 6))
            {
                if (o != this && o is Mobile t && CanBeHarmful(t, false))
                    targets.Add(t);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(t, this, dmg, 0, 0, 0, 0, 100);

                t.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

                // 30% chance to apply brief paralysis
                if (Utility.RandomDouble() < 0.3 && t is Mobile targetMobile)
                {
                    targetMobile.Paralyze(TimeSpan.FromSeconds(1.5));
                }
            }
        }

        // ——— Bubble Rift: drop toxic gas tiles around the combatant ———
        private void BubbleRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Bubbles of oblivion!*");
            PlaySound(0x1E6);

            var center = target.Location;
            Effects.SendLocationParticles(EffectItem.Create(center, Map, EffectItem.DefaultDuration), 0x3728, 8, 10, UniqueHue, 0, 5039, 0);

            // After a short delay, spawn gas hazards
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    int xOff = Utility.RandomMinMax(-2, 2), yOff = Utility.RandomMinMax(-2, 2);
                    var loc = new Point3D(center.X + xOff, center.Y + yOff, center.Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x377A, 5, 15, UniqueHue, 0, 5032, 0);
                }
            });
        }

        // ——— Arcane Cascade: chain of mini‐orb explosions ———
        private void ArcaneCascade()
        {
            if (!(Combatant is Mobile initial) || !CanBeHarmful(initial, false)) return;

            Say("*Feel the cascade!*");
            PlaySound(0x20B);

            var chainTargets = new List<Mobile> { initial };
            int max = 4;
            double range = 6.0;

            // find up to max additional targets
            for (int i = 0; i < max; i++)
            {
                var last = chainTargets[chainTargets.Count - 1];
                Mobile next = null;
                double bestDist = double.MaxValue;

                foreach (var o in Map.GetMobilesInRange(last.Location, (int)range))
                {
                    if (o != this && o is Mobile m && !chainTargets.Contains(m) && CanBeHarmful(m, false))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < bestDist)
                        {
                            bestDist = d;
                            next = m;
                        }
                    }
                }

                if (next != null)
                    chainTargets.Add(next);
                else
                    break;
            }

            // fire the chain
            for (int i = 0; i < chainTargets.Count; i++)
            {
                var src = (i == 0 ? (Mobile)this : chainTargets[i - 1]);
                var dst = chainTargets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                int damage = Utility.RandomMinMax(25, 40);
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, damage, 0, 0, 0, 0, 100);
                        dst.FixedParticles(0x374A, 5, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // ——— Death Explosion ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*Effervescence... released!*");
                Effects.PlaySound(Location, Map, 0x211);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // spawn random hazard tiles
                int count = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < count; i++)
                {
                    int xOff = Utility.RandomMinMax(-3, 3), yOff = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + xOff, Y + yOff, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = new ManaDrainTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(EffectItem.Create(loc, Map, EffectItem.DefaultDuration), 0x376A, 5, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // ——— Loot ———
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 14));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new CloakOfEffervescentMantra()); // unique drop!
        }

        // ——— Utility ———
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public MysticEffervescence(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reset timers
            m_NextSurgeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBubbleTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextCascadeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
        }
    }
}
