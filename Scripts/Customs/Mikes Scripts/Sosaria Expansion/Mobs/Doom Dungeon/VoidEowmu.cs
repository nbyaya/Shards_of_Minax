using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;        // for spell effects
using Server.Spells.Fourth; // if you want to reuse existing spell visuals
using Server.Spells.Seventh;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a void eowmu corpse")]
    public class VoidEowmu : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextVoidPulse;
        private DateTime m_NextVoidRift;
        private DateTime m_NextSummon;
        private DateTime m_NextLifeSiphon;

        // Track last location for aura
        private Point3D m_LastLocation;
        
        // Unique void purple hue
        private const int VoidHue = 1175;

        [Constructable]
        public VoidEowmu() : base(
            AIType.AI_Mage, 
            FightMode.Closest, 
            12, 1, 0.2, 0.4
        )
        {
            Name            = "the Void Eowmu";
            Body            = 1440;       // same as the original Eowmu
            BaseSoundID     = 0xA8;
            Hue             = VoidHue;

            // — Stats —
            SetStr( 600,  700 );
            SetDex( 200,  250 );
            SetInt( 700,  800 );

            SetHits( 2500, 3000 );
            SetStam( 300,  350 );
            SetMana( 1000, 1200 );

            SetDamage( 20,  30 );

            // — Damage Types —
            SetDamageType( ResistanceType.Physical, 30 );
            SetDamageType( ResistanceType.Energy,   70 );

            // — Resistances —
            SetResistance( ResistanceType.Physical, 50, 60 );
            SetResistance( ResistanceType.Fire,     60, 70 );
            SetResistance( ResistanceType.Cold,     60, 70 );
            SetResistance( ResistanceType.Poison,   40, 50 );
            SetResistance( ResistanceType.Energy,   90, 95 );

            // — Skills —
            SetSkill( SkillName.EvalInt,      120.0, 130.0 );
            SetSkill( SkillName.Magery,       120.0, 130.0 );
            SetSkill( SkillName.MagicResist,  125.0, 135.0 );
            SetSkill( SkillName.Meditation,   110.0, 120.0 );
            SetSkill( SkillName.Tactics,       95.0, 105.0 );
            SetSkill( SkillName.Wrestling,     95.0, 105.0 );

            Fame       = 35000;
            Karma     = -35000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize ability timers
            m_NextVoidPulse  = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextVoidRift   = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextSummon     = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextLifeSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(15);

            m_LastLocation = this.Location;
        }

        // — Aura: Void Scream — drains stamina and deals minor energy damage
        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            base.OnMovement(m, oldLocation);

            if ( m == this || !Alive || this.Map == Map.Internal )
                return;

            if ( m.InRange( this.Location, 3 ) && m is Mobile target && CanBeHarmful(target, false) )
            {
                DoHarmful(target);

                // Stamina drain
                int drain = Utility.RandomMinMax(5, 15);
                if ( target.Stam >= drain )
                {
                    target.Stam -= drain;
                    target.SendMessage( 0x482, "You feel your strength ebb away!" );
                    target.FixedParticles( 0x373A, 10, 20, 5032, VoidHue, 0, EffectLayer.Waist );
                }

                // Minor energy damage
                AOS.Damage( target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100 );
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( !Alive || Combatant == null || Map == Map.Internal )
                return;

            var now = DateTime.UtcNow;

            // — Life Siphon — single target heal
            if ( now >= m_NextLifeSiphon && this.InRange( Combatant.Location, 8 ) )
            {
                LifeSiphon();
                m_NextLifeSiphon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
                return;
            }

            // — Void Pulse — AoE around self
            if ( now >= m_NextVoidPulse )
            {
                VoidPulse();
                m_NextVoidPulse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                return;
            }

            // — Void Rift — hazardous tile at target
            if ( now >= m_NextVoidRift )
            {
                VoidRift();
                m_NextVoidRift = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            // — Summon Void Wraiths —
            if ( now >= m_NextSummon )
            {
                SummonVoidWraiths();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
                return;
            }
        }

        // — Ability #1: Life Siphon — drain HP and heal self
        private void LifeSiphon()
        {
            if ( Combatant is Mobile target && CanBeHarmful(target, false) )
            {
                this.Say( "*The void consumes you...*" );
                PlaySound( 0x1F3 );

                int dmg = Utility.RandomMinMax(40, 60);
                DoHarmful(target);
                AOS.Damage( target, this, dmg, 0, 0, 0, 0, 100 );

                // Heal self
                this.Heal( Utility.RandomMinMax(30, 50) );

                // Visual effect
                target.FixedParticles( 0x374A, 10, 15, 5032, VoidHue, 0, EffectLayer.Head );
            }
        }

        // — Ability #2: Void Pulse — AoE burst + poison ground
        private void VoidPulse()
        {
            this.Say( "*Feel the void's wrath!*" );
            PlaySound( 0x211 );

            Effects.SendLocationParticles(
                EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ),
                0x375A, 12, 20, VoidHue, 0, 5039, 0
            );

            var targets = new List<Mobile>();
            foreach ( var m in this.Map.GetMobilesInRange(this.Location, 6) )
            {
                if ( m != this && CanBeHarmful(m, false) )
                    targets.Add(m);
            }

            foreach ( var tgt in targets )
            {
                DoHarmful(tgt);
                int damage = Utility.RandomMinMax(30, 50);
                AOS.Damage( tgt, this, damage, 0, 0, 0, 0, 100 );
                tgt.FixedParticles( 0x3790, 8, 16, 5032, VoidHue, 0, EffectLayer.Waist );
            }

            // Poison tile hazard
            var tile = new PoisonTile();
            tile.Hue = VoidHue;
            tile.MoveToWorld( this.Location, this.Map );
        }

        // — Ability #3: Void Rift — drops a lasting vortex at the target
        private void VoidRift()
        {
            if ( Combatant == null || Map == Map.Internal )
                return;

            Point3D loc = Combatant.Location;
            this.Say( "*Tear reality asunder!*" );
            PlaySound( 0x22F );

            Effects.SendLocationParticles(
                EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                0x3728, 8, 10, VoidHue, 0, 5039, 0
            );

            Timer.DelayCall( TimeSpan.FromSeconds(0.7), () =>
            {
                if ( this.Map == null ) return;
                var vortex = new VortexTile();
                vortex.Hue = VoidHue;
                vortex.MoveToWorld( loc, this.Map );
                Effects.PlaySound( loc, this.Map, 0x1F6 );
            });
        }

        // — Ability #4: Summon lesser Void Wraiths —
        private void SummonVoidWraiths()
        {
            this.Say( "*Arise, my void spawn!*" );
            PlaySound( 0x1EA );

            int count = Utility.RandomMinMax(2, 4);
            for ( int i = 0; i < count; i++ )
            {
                var w = new BaseCreature( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
                {
                    Name       = "void wraith",
                    Body       = 159,        // spectral ghost body
                    Hue        = VoidHue,
                    BaseSoundID = 0x482
                };

                w.SetStr( 200, 250 );
                w.SetDex( 150, 200 );
                w.SetInt( 100, 150 );
                w.SetHits( 300, 350 );
                w.SetDamage( 10, 15 );
                w.SetDamageType( ResistanceType.Energy, 100 );
                w.SetResistance( ResistanceType.Physical, 30, 40 );
                w.SetResistance( ResistanceType.Energy, 50, 60 );
                w.SetSkill( SkillName.Magery, 80.0, 90.0 );
                w.SetSkill( SkillName.MagicResist, 50.0, 60.0 );
                w.VirtualArmor = 30;

                // position around boss
                Point3D spawn = new Point3D(
                    this.X + Utility.RandomMinMax(-2,2),
                    this.Y + Utility.RandomMinMax(-2,2),
                    this.Z
                );
                w.MoveToWorld( spawn, this.Map );
            }
        }

        // — Death Explosion & Hazards —
        public override void OnDeath( Container c )
        {
            if ( Map != null )
            {
                this.Say( "*The void...claims me...*" );
                Effects.PlaySound( this.Location, this.Map, 0x211 );
                Effects.SendLocationParticles(
                    EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ),
                    0x3709, 15, 40, VoidHue, 0, 5052, 0
                );

                // Scatter random void hazards
                for ( int i = 0; i < Utility.RandomMinMax(4, 7); i++ )
                {
                    int dx = Utility.RandomMinMax(-4,4), dy = Utility.RandomMinMax(-4,4);
                    var loc = new Point3D( X+dx, Y+dy, Z );
                    if ( !Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false) )
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var hazard = new NecromanticFlamestrikeTile();
                    hazard.Hue = VoidHue;
                    hazard.MoveToWorld( loc, this.Map );
                }
            }

            base.OnDeath(c);
        }

        // — Loot & Properties —
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public override void GenerateLoot()
        {
            AddLoot( LootPack.UltraRich, 2 );
            AddLoot( LootPack.Gems,        Utility.RandomMinMax(10, 15) );
            AddLoot( LootPack.HighScrolls, Utility.RandomMinMax(2, 4) );

            if ( Utility.RandomDouble() < 0.05 )
                PackItem( new VoidCore() ); // your custom rare drop
        }

        public VoidEowmu( Serial serial ) : base(serial) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // re-init timers on load
            m_NextLifeSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextVoidPulse  = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextVoidRift   = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextSummon     = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }
    }
}
