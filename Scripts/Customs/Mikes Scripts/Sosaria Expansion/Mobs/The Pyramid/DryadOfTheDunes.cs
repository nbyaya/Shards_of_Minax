using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a dryad of the dunes corpse")]
    public class DryadOfTheDunes : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextAuraTime, m_NextBlastTime, m_NextEntombTime, m_NextVeilTime, m_NextShiftTime;
        private Point3D m_LastLocation;

        // Unique sandy‐gold hue
        private const int UniqueHue = 1899;

        [Constructable]
        public DryadOfTheDunes()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dryad of the dunes";
            Body = 266;
            BaseSoundID = 0x57B;
            Hue = UniqueHue;

            // --- Stats ---
            SetStr(200, 250);
            SetDex(180, 220);
            SetInt(350, 450);

            SetHits(800, 900);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   35, 45);

            // Skills
            SetSkill(SkillName.Magery,       100.1, 115.0);
            SetSkill(SkillName.EvalInt,      100.1, 115.0);
            SetSkill(SkillName.MagicResist,  120.0, 130.0);
            SetSkill(SkillName.Meditation,    90.0, 100.0);
            SetSkill(SkillName.Tactics,       80.1,  90.0);
            SetSkill(SkillName.Wrestling,     80.1,  90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Loot: desert reagents + chance at a Sandwoven Circlet
            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            if (Utility.RandomDouble() < 0.005) // 0.5%
                PackItem(new LeggingsOfTheGhostBell());

            // Initialize timers
            m_NextAuraTime   = DateTime.UtcNow;
            m_NextBlastTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextEntombTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextVeilTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
            m_NextShiftTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;
        }

        // --- Leaves quicksand behind as she moves ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation)
            {
                m_LastLocation = this.Location;

                if (Utility.RandomDouble() < 0.3)
                {
                    // Spawn a quicksand hazard at her previous spot
                    Point3D p = oldLocation;
                    if (!Map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                        p.Z = Map.GetAverageZ(p.X, p.Y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(p, Map);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // 1) Desiccating Aura every 5s
            if (DateTime.UtcNow >= m_NextAuraTime)
            {
                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
                AuraOfHeat();
            }

            // If we're not fighting, bail
            if (!(Combatant is Mobile target) || !Alive || Map == Map.Internal)
                return;

            // 2) Sand Blast AoE
            if (DateTime.UtcNow >= m_NextBlastTime && InRange(target, 8))
            {
                m_NextBlastTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                SandBlast();
            }
            // 3) Entomb in Sand (paralyze + quicksand under feet)
            else if (DateTime.UtcNow >= m_NextEntombTime && InRange(target, 12))
            {
                m_NextEntombTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                EntombInSand(target);
            }
            // 4) Sandstorm Veil (self‑invisibility + blind nearby)
            else if (DateTime.UtcNow >= m_NextVeilTime && InRange(target, 12))
            {
                m_NextVeilTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
                SandstormVeil();
            }
            // 5) Dune Shift (teleport behind the target)
            else if (DateTime.UtcNow >= m_NextShiftTime && InRange(target, 14))
            {
                m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
                DuneShift(target);
            }
        }

        // --- 1) Passive Desiccating Aura ---
        private void AuraOfHeat()
        {
            var eable = Map.GetMobilesInRange(Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && m.Alive && CanBeHarmful(m, false))
                {
                    int drain = Utility.RandomMinMax(5, 10);
                    if (m.Stam >= drain)
                    {
                        m.Stam -= drain;
                        m.SendMessage("The dryad's scorching aura saps your strength!");
                        AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                    }
                }
            }
            eable.Free();
        }

        // --- 2) Sand Blast: 360° cone of physical+fire damage ---
        private void SandBlast()
        {
            PlaySound(0x20A);
            FixedParticles(0x373A, 1, 30, 9502, UniqueHue, 0, EffectLayer.CenterFeet);

            var hits = new List<Mobile>();
            var eable = Map.GetMobilesInRange(Location, 8);
            foreach (Mobile m in eable)
                if (m != this && CanBeHarmful(m, false))
                    hits.Add(m);
            eable.Free();

            foreach (var m in hits)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(m, this, dmg, 10, 0, 0, 0, 90); // 90% fire
            }
        }

        // --- 3) Entomb in Sand: paralyze + quicksand under target ---
        private void EntombInSand(Mobile target)
        {
            PlaySound(0x2E4);
            target.SendMessage("The dunes surge up, trapping your limbs in sand!");
            DoHarmful(target);

            // Place a quicksand tile under them
            var qs = new QuicksandTile();
            qs.Hue = UniqueHue;
            qs.MoveToWorld(target.Location, Map);

            // Paralyze briefly
            target.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6)));
        }

        // --- 4) Sandstorm Veil: hide and blind nearby foes ---
        private void SandstormVeil()
        {
            Say("*The sands shall conceal me!*");
            PlaySound(0x1FE);
            Hidden = true;

            // Dust blast on nearby players
            var eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && m.Alive && CanBeHarmful(m, false))
                {
                    m.SendMessage("A blinding sandstorm erupts!");
                    m.FixedParticles(0x375A, 10, 15, 5012, EffectLayer.Head);
                }
            }
            eable.Free();

            // Reappear after 6–10 seconds
            Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10)), () =>
            {
                if (!Deleted && Alive)
                {
                    Hidden = false;
                    PlaySound(0x1FD);
                    FixedParticles(0x375E, 1, 30, 2053, UniqueHue, 0, EffectLayer.Waist);
                }
            });
        }

        // --- 5) Dune Shift: blink behind the target ---
        private void DuneShift(Mobile target)
        {
            Say("*You cannot escape the shifting sands!*");
            PlaySound(0x1F7);

            // Compute a spot just behind the target
            int dx = X < target.X ? -1 : 1;
            int dy = Y < target.Y ? -1 : 1;
            var dest = new Point3D(target.X + dx, target.Y + dy, target.Z);

            // Validate Z
            if (!Map.CanFit(dest.X, dest.Y, dest.Z, 16, false, false))
                dest.Z = Map.GetAverageZ(dest.X, dest.Y);

            // Visuals at old and new
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);
            MoveToWorld(dest, Map);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Combatant = target;
        }

        // --- On death: a final sand nova + quicksand hazards ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Scatter 3–5 quicksand traps
                for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
                {
                    int x = X + Utility.RandomMinMax(-3, 3);
                    int y = Y + Utility.RandomMinMax(-3, 3);
                    int z = Z;
                    var p = new Point3D(x, y, z);
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        p.Z = Map.GetAverageZ(x, y);

                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(p, Map);
                }
            }

            base.OnDeath(c);
        }

        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty  { get { return 120.0; } }
        public override double DispelFocus       { get { return 60.0;  } }

        public DryadOfTheDunes(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
