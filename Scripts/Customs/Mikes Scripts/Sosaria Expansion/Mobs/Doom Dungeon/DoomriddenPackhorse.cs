using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a doomridden packhorse corpse")]
    public class DoomriddenPackhorse : BaseCreature
    {
        private DateTime m_NextHauntTime;
        private DateTime m_NextStampedeTime;
        private DateTime m_NextGloomCurseTime;
        private Point3D m_LastLocation;

        // Eerie blood‐red hue
        private const int UniqueHue = 1172;

        [Constructable]
        public DoomriddenPackhorse()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a doomridden packhorse";
            Body = 291;
            BaseSoundID = 0xA8;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(100, 120);
            SetInt(100, 120);

            SetHits(1000, 1200);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Fire, 30, 40);

            // Skills
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 10000;
            Karma = -10000;
            VirtualArmor = 70;
            ControlSlots = 5;

            // Ability cooldowns
            m_NextHauntTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStampedeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextGloomCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            m_LastLocation = this.Location;

            // Basic reagent loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
            PackItem(new SpidersSilk   (Utility.RandomMinMax(10, 20)));
        }

        // Haunting aura: drains stamina when players move too close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    int drain = Utility.RandomMinMax(5, 15);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage("You feel your strength ebbing under the doomridden aura!");
                        target.FixedParticles(0x3779, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x2F);
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // Core AI loop: checks for ability cooldowns and leaves spectral hoofprints
        public override void OnThink()
        {
            base.OnThink();

            // Haunting Shadow AoE
            if (DateTime.UtcNow >= m_NextHauntTime)
            {
                HauntingShadow();
                m_NextHauntTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }

            // Ghostly Stampede (melee burst)
            if (Combatant is Mobile target1 && DateTime.UtcNow >= m_NextStampedeTime && InRange(target1.Location, 8))
            {
                GhostlyStampede(target1);
                m_NextStampedeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }

            // Gloom Curse (single‐target lethal poison)
            if (Combatant is Mobile target2 && DateTime.UtcNow >= m_NextGloomCurseTime && InRange(target2.Location, 12))
            {
                GloomCurseAttack(target2);
                m_NextGloomCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }

            // Spectral hoofprint on movement
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.30)
            {
                var oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    var tile = new NecromanticFlamestrikeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int z = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, z, 16, false, false))
                    {
                        var tile = new NecromanticFlamestrikeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, z), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- Ability #1: Haunting Shadow ---
        private void HauntingShadow()
        {
            if (Map == null) return;

            Say("*A chilling wail echoes through the dungeon…*");
            PlaySound(0x482);

            var targets = new List<Mobile>();
            var eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && m is Mobile tgt)
                    targets.Add(tgt);
            }
            eable.Free();

            foreach (var tgt in targets)
            {
                DoHarmful(tgt);
                tgt.FixedParticles(0x3709, 10, 20, 5032, UniqueHue, 0, EffectLayer.Waist);
                AOS.Damage(tgt, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);

                if (Utility.RandomDouble() < 0.30)
                {
                    // Spawn a lingering poison tile under the target
                    var p = new PoisonTile();
                    p.Hue = UniqueHue;
                    p.MoveToWorld(tgt.Location, this.Map);
                }
            }
        }

        // --- Ability #2: Ghostly Stampede ---
        private void GhostlyStampede(Mobile target)
        {
            Say("*Feel the thunder of doomridden hooves!*");
            for (int i = 0; i < 3; i++)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3966, 8, 20, UniqueHue, 0, 5021, 0
                );
                PlaySound(0x15E);

                if (target != null && CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
                }
            }
        }

        // --- Ability #3: Gloom Curse ---
        private void GloomCurseAttack(Mobile target)
        {
            if (!CanBeHarmful(target, false)) return;

            Say("*Embrace the weight of sorrow…*");
            PlaySound(0x228);
            target.FixedParticles(0x375A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

            // Guaranteed lethal poison
            target.Poison = Poison.Lethal;
        }

        // Death explosion + residual poison clouds
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The doomridden packhorse collapses into writhing shadows…*");
                PlaySound(0x20F);

                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0
                );

                // Scatter poison tiles around the corpse
                int count = Utility.RandomMinMax(3, 6);
                for (int i = 0; i < count; i++)
                {
                    var loc = new Point3D(
                        X + Utility.RandomMinMax(-3, 3),
                        Y + Utility.RandomMinMax(-3, 3),
                        Z
                    );

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);
                        if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                            continue;
                    }

                    var p = new PoisonTile();
                    p.Hue = UniqueHue;
                    p.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // Loot & misc overrides
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));

            // 2% chance for a Beastmaster's Saddle
            if (Utility.RandomDouble() < 0.02)
                PackItem(new BeastmastersSaddle());
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus      { get { return  60.0; } }

        // Preserve pack‐horse yields
        public override int Meat { get { return 6; } }
        public override int Hides { get { return 15; } }

        public DoomriddenPackhorse(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers on load
            m_NextHauntTime       = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStampedeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextGloomCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_LastLocation       = this.Location;
        }
    }
}
