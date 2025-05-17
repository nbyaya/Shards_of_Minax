using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a phantom cat corpse")]
    public class PhantomCat : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextPhaseTime;
        private DateTime m_NextPounceTime;
        private DateTime m_NextCloneTime;

        // Track last location for aura/tile effects
        private Point3D m_LastLocation;

        // Ghostly blue-white hue
        private const int UniqueHue = 1158;

        // Dummy property to satisfy PhaseShift's obstacle‑walking toggle
        public bool CanWalkOverObstacles { get; set; }

        [Constructable]
        public PhantomCat()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name         = "a Phantom Cat";
            Body         = 1441;   // skeletal cat body
            BaseSoundID  = 229;    // skeletal cat sound
            Hue          = UniqueHue;

            // Stats
            SetStr(350, 400);
            SetDex(300, 350);
            SetInt(200, 250);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(0);

            SetDamage(12, 18);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold,     50);
            SetDamageType(ResistanceType.Energy,   30);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     20, 30);
            SetResistance(ResistanceType.Cold,     70, 80);
            SetResistance(ResistanceType.Poison,   40, 50);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.Tactics,       100.0, 110.0);
            SetSkill(SkillName.Wrestling,     100.0, 110.0);
            SetSkill(SkillName.MagicResist,    80.0,  90.0);

            Fame          = 18000;
            Karma         = -18000;
            VirtualArmor  = 75;
            ControlSlots  = 5;

            // Initialize cooldowns
            m_NextPhaseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCloneTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new Bone(Utility.RandomMinMax(20, 30)));
            PackItem(new Hides(Utility.RandomMinMax(15, 25)));
        }

        // Aura: drains stamina from anyone moving within 2 tiles
        public override void OnMovement(Mobile moved, Point3D oldLocation)
        {
            if (moved != null && moved != this && moved.Map == this.Map && moved.InRange(this.Location, 2) && Alive && CanBeHarmful(moved, false))
            {
                if (moved is Mobile target)
                {
                    DoHarmful(target);
                    int stamDrain = Utility.RandomMinMax(5, 10);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x480, "A chilling breath saps your strength!");
                        target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        target.PlaySound(0x482);
                    }
                }
            }
            base.OnMovement(moved, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Phase Shift: brief incorporeal invulnerability and movement burst
            if (DateTime.UtcNow >= m_NextPhaseTime)
            {
                PhaseShift();
                m_NextPhaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Pounce Attack: leap to target
            else if (DateTime.UtcNow >= m_NextPounceTime && this.InRange(Combatant.Location, 12))
            {
                PounceAttack();
                m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Clone Summon: spawn illusionary phantoms
            else if (DateTime.UtcNow >= m_NextCloneTime)
            {
                SummonClones();
                m_NextCloneTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }

            // Leave phantom tile trail occasionally
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                DropSpectralTile(m_LastLocation);
            }
            m_LastLocation = this.Location;
        }

        private void PhaseShift()
        {
            this.Say("*Silence of the grave...*");
            PlaySound(0x4F2); // ghostly whoosh
            this.FixedParticles(0x3709, 1, 30, 9502, UniqueHue, 0, EffectLayer.Waist);

            // Become "ghostly" for 3 seconds
            this.CanWalkOverObstacles = true;
            this.Hidden = true;

            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
            {
                if (!Deleted && Alive)
                {
                    this.Hidden = false;
                    this.CanWalkOverObstacles = false;
                    this.Say("*I return...*");
                    PlaySound(0x4F3);
                }
            });
        }

        private void PounceAttack()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            this.Say("*Leap of shadows!*");
            PlaySound(0x48D); // cat pounce
            this.MoveToWorld(target.Location, target.Map);

            DoHarmful(target);
            int dmg = Utility.RandomMinMax(60, 90);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

            // Knockback
            Point3D knockLoc = new Point3D(
                target.X + (target.X - this.X),
                target.Y + (target.Y - this.Y),
                target.Z);
            if (target.Map.CanFit(knockLoc.X, knockLoc.Y, knockLoc.Z, 16, false, false))
                target.Location = knockLoc;
        }

        private void SummonClones()
        {
            this.Say("*Arise, my phantoms!*");
            PlaySound(0x482);

            for (int i = 0; i < 3; i++)
            {
                Point3D spawn = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z);

                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                // Use BaseCreature type so we can call SetStr, SetDex, etc.
                BaseCreature clone = new BaseCreature(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
                {
                    Name         = "phantom catling",
                    Body         = 1441,
                    Hue          = UniqueHue,
                    BaseSoundID  = 229,
                    Hidden       = true,
                    Blessed      = false,
                    Karma        = -1000,
                    Fame         = 500,
                    VirtualArmor = 20
                };

                clone.SetStr(100, 120);
                clone.SetDex(200, 220);
                clone.SetHits(150);
                clone.SetDamage(8, 12);

                clone.MoveToWorld(spawn, Map);

                // Fade in
                clone.FixedParticles(0x3709, 5, 20, 9502, UniqueHue, 0, EffectLayer.Waist);

                // Auto-delete after 12 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(12), () =>
                {
                    if (clone != null && !clone.Deleted)
                        clone.Delete();
                });
            }
        }

        private void DropSpectralTile(Point3D loc)
        {
            var tile = new VortexTile(); // ethereal disturbance
            tile.Hue = UniqueHue;
            tile.MoveToWorld(loc, this.Map);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            this.Say("*The veil... rends...*");
            PlaySound(0x4F4);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 60, UniqueHue, 0, 9502, 0);

            // Spawn poisonous fumes around corpse
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                var fume = new PoisonTile();
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);
                fume.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // Loot & other properties
        public override bool BleedImmune     => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus      => 60.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            // 3% chance for a spectral artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new SpectralClaw());
        }

        public PhantomCat(Serial serial) : base(serial)
        {
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

            // reset timers on load
            m_NextPhaseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextCloneTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_LastLocation   = this.Location;
        }
    }
}
