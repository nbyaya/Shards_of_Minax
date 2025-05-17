using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a daemonic ferret corpse")]
    public class DaemonicFerret : BaseCreature
    {
        private DateTime m_NextHellfire;
        private DateTime m_NextSummon;
        private bool m_CanScream;
        private Point3D m_LastLocation;

        // Fiery-red hue for infernal flair
        private const int UniqueHue = 1175;

        [Constructable]
        public DaemonicFerret() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name           = "a daemonic ferret";
            Body           = 0x117;
            Hue            = UniqueHue;
            // (ferret uses its own small "dook" sounds)
            
            // --- Stats ---
            SetStr(200, 250);
            SetDex(150, 180);
            SetInt(300, 350);

            SetHits(1000, 1200);
            SetStam(150, 180);
            SetMana(400, 500);

            SetDamage(20, 25);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt,      100.0, 110.0);
            SetSkill(SkillName.Magery,       100.0, 110.0);
            SetSkill(SkillName.MagicResist,  110.0, 120.0);
            SetSkill(SkillName.Tactics,       90.0, 100.0);
            SetSkill(SkillName.Wrestling,     90.0, 100.0);

            Fame           = 15000;
            Karma         = -15000;
            VirtualArmor  = 60;
            ControlSlots  = 4;

            // Ability cooldowns
            m_NextHellfire = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSummon   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_CanScream    = true;
            m_LastLocation = this.Location;

            // Base loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
        }

        public DaemonicFerret(Serial serial) : base(serial) { }

        // --- Cursed Screech on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m_CanScream && m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && CanBeHarmful(m, false))
            {
                m_CanScream = false;

                this.Say("*scree!*");
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    target.ApplyPoison(this, Poison.Lesser);
                    target.SendMessage(0x22, "A demonic screech warps your veins!");
                }

                // reset scream after 25–35s
                Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35)), () => m_CanScream = true);
            }
        }

        // --- Main AI Loop: Hellfire Breath, Summon, Flaming Trail ---
        public override void OnThink()
        {
            base.OnThink();

            // Flaming Trail
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.3 && Map != null && Map != Map.Internal)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    FlamestrikeHazardTile tile = new FlamestrikeHazardTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(old, Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // No combat or invalid map?
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Hellfire Breath (AoE cone)
            if (DateTime.UtcNow >= m_NextHellfire && Combatant is Mobile target && InRange(target.Location, 10))
            {
                m_NextHellfire = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
                HellfireBreath(target);
            }
            // Summon Fiendish Ferretlings
            else if (DateTime.UtcNow >= m_NextSummon)
            {
                m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
                SummonFerretlings();
            }
        }

        // --- Hellfire Breath: cone of fire + lingering hazard ---
        private void HellfireBreath(Mobile target)
        {
            this.Say("*fiery dook!*");
            PlaySound(0x208);
            Point3D sourceLoc = this.Location;

            // Affect everyone in a 90° arc in front
            foreach (Mobile m in Map.GetMobilesInRange(sourceLoc, 8))
            {
                if (m != this && CanBeHarmful(m, false) && m.InLOS(this) && this.InRange(m, 8))
                {
                    // Simple cone check via direction
                    var dirTo = this.GetDirectionTo(m);
                    if (Math.Abs((int)dirTo - (int)this.Direction) <= 2)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);

                        // Drop a patch of fire under them
                        if (Map.CanFit(m.X, m.Y, m.Z, 16, false, false))
                        {
                            HotLavaTile lava = new HotLavaTile();
                            lava.Hue = UniqueHue;
                            lava.MoveToWorld(m.Location, Map);
                        }
                    }
                }
            }

            // Particle cone effect
            Effects.SendLocationEffect(
                sourceLoc, 
                Map, 
                0x3709, 
                12, 
                30, 
                UniqueHue, 
                0);
        }

        // --- Summon smaller Daemonic Ferretlings ---
        private void SummonFerretlings()
        {
            this.Say("*spawn!*");
            PlaySound(0x1F3);

            int count = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < count; i++)
            {
                var baby = new Ferret();         // reuse Base Ferret
                baby.Hue = UniqueHue;
                baby.Name = "a daemonic ferretling";
                baby.Team = this.Team;
                baby.ControlSlots = 0;
                baby.SetStr(50, 60);
                baby.SetDex(80, 90);
                baby.SetInt(50, 60);
                baby.SetHits(60, 75);
                baby.SetDamage(8, 12);
                baby.SetResistance(ResistanceType.Fire, 60, 70);
                baby.MoveToWorld(
                    new Point3D(
                        X + Utility.RandomMinMax(-2, 2),
                        Y + Utility.RandomMinMax(-2, 2),
                        Z),
                    Map
                );
            }
        }

        // --- Death Explosion & Hazard Field ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*the inferno... fades...*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0
                );

                // Scatter poison clouds
                for (int i = 0; i < 5; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    PoisonTile gas = new PoisonTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot & Other Overrides ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new StridersEmberweave()); // example unique drop
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 120.0;
        public override double DispelFocus     => 60.0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextHellfire = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextSummon   = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_CanScream    = true;
            m_LastLocation = this.Location;
        }
    }
}
