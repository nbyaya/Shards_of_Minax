using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a tomb ambusher corpse")]
    public class TombAmbusher : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextBarrageTime;
        private DateTime m_NextMiasmaTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextStealthTime;
        private Point3D m_LastLocation;

        // Unique Hue – an eerie bone‑white with a greenish tint
        private const int UniqueHue = 2123;

        [Constructable]
        public TombAmbusher()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a tomb ambusher";
            Body = 726;                // same body as KepetchAmbusher
            Hidden = true;             // starts hidden
            Hue = UniqueHue;

            // Core stats
            SetStr(350, 380);
            SetDex(180, 200);
            SetInt(500, 550);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(12, 18);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison,   40);
            SetDamageType(ResistanceType.Cold,     30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     65, 75);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics,     90.0, 100.0);
            SetSkill(SkillName.Wrestling,   90.0, 100.0);
            SetSkill(SkillName.Magery,     100.0, 120.0);
            SetSkill(SkillName.EvalInt,     90.0, 100.0);
            SetSkill(SkillName.Meditation,  80.0,  90.0);
            SetSkill(SkillName.Stealth,    125.0);
            SetSkill(SkillName.Hiding,     125.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize cooldowns
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextMiasmaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextRiftTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextStealthTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_LastLocation    = this.Location;

            // Standard loot
            PackItem(new Bone(Utility.RandomMinMax(10, 20)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackGold(500, 700);
        }

        // Allows this creature to vanish into stealth
        public override bool CanStealth { get { return true; } }

        // Sound overrides (same as KepetchAmbusher)
        public override int GetIdleSound()  { return 1545; }
        public override int GetAngerSound() { return 1542; }
        public override int GetHurtSound()  { return 1544; }
        public override int GetDeathSound() { return 1543; }

        // Reveal if damaged
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            RevealingAction();
            base.OnDamage(amount, from, willKill);
        }
        public override void OnDamagedBySpell(Mobile from)
        {
            RevealingAction();
            base.OnDamagedBySpell(from);
        }

        // Aura: lays trap webs when a target moves near
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && CanBeHarmful(m, false))
            {
                // occasionally spawn a sticky trap
                if (Utility.RandomDouble() < 0.20)
                {
                    TrapWeb web = new TrapWeb();
                    web.Hue = UniqueHue;
                    web.MoveToWorld(m.Location, m.Map);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // Main AI loop: re-stealth, and fire off special attacks
        public override void OnThink()
        {
            base.OnThink();
            if (Deleted || !Alive || Map == null || Map == Map.Internal)
                return;

            // Re-hide periodically
            if (!Hidden && DateTime.UtcNow >= m_NextStealthTime)
            {
                HideSelf();
                m_NextStealthTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }

            // Cursed Miasma: periodic poison AoE
            if (DateTime.UtcNow >= m_NextMiasmaTime)
            {
                CursedMiasma();
                m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }

            // Offense only if there's a valid Mobile combatant
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                // Bone Shard Barrage
                if (DateTime.UtcNow >= m_NextBarrageTime && InRange(target.Location, 10))
                {
                    BoneShardBarrage(target);
                    m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
                // Shadow Rift seeding
                else if (DateTime.UtcNow >= m_NextRiftTime && InRange(target.Location, 12))
                {
                    ShadowRift(target);
                    m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
            }
        }

        private void HideSelf()
        {
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);
            PlaySound(0x22F);
            Hidden = true;
            UseSkill(SkillName.Stealth);
        }

        // --- Ability 1: Cursed Miasma (AoE poison burst + spawn PoisonTile) ---
        private void CursedMiasma()
        {
            this.Say("*Feel the tomb’s curse!*");
            PlaySound(0x1E3);

            var affected = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    affected.Add(m);
            }

            // Visual central puff
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 30, UniqueHue, 0, 5032, 0);

            foreach (Mobile m in affected)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100); // pure poison
                m.ApplyPoison(this, Poison.Lethal);
            }

            // Spawn lingering poison gas clouds
            for (int i = 0; i < 4; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    PoisonTile gas = new PoisonTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Ability 2: Bone Shard Barrage (chain projectile) ---
        private void BoneShardBarrage(Mobile initial)
        {
            this.Say("*Your bones shall shatter!*");
            PlaySound(0x1F3);

            var targets = new List<Mobile> { initial };
            var last = initial;
            int maxBounces = 4, range = 6;

            // Find up to maxBounces additional nearby targets
            for (int i = 0; i < maxBounces; i++)
            {
                Mobile next = null; double best = -1;
                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && m != last && !targets.Contains(m) && m.Alive && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (best < 0 || d < best) { best = d; next = m; }
                    }
                }
                if (next == null) break;
                targets.Add(next);
                last = next;
            }

            // Launch the chain of bone-shard projectiles
            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

                var victim = dst;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                {
                    if (victim.Alive && CanBeHarmful(victim, false))
                    {
                        DoHarmful(victim);
                        AOS.Damage(victim, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);

                    }
                });
            }
        }

        // --- Ability 3: Shadow Rift (delayed hazard at target spot) ---
        private void ShadowRift(Mobile target)
        {
            this.Say("*Rise from the crypt!*");
            PlaySound(0x209);

            var loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
            {
                if (Map == null) return;

                // Validate a fit spot
                var z = loc.Z;
                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                var necro = new NecromanticFlamestrikeTile();
                necro.Hue = UniqueHue;
                necro.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
                Effects.PlaySound(loc, Map, 0x2F3);
            });
        }

        // Death explosion: spawn quicksand traps
        public override void OnDeath(Container c)
        {
            
			base.OnDeath(c);
			if (Map == null) return;
			this.Say("*The sands reclaim you...*");
            PlaySound(0x1E2);

            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                QuicksandTile qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(loc, Map);
            }

            
        }

        // Serialization boilerplate
        public TombAmbusher(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers after load
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextMiasmaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextRiftTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextStealthTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_LastLocation    = Location;
        }
    }
}
