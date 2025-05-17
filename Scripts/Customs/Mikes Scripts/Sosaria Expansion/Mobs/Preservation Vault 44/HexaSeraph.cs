using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;           // for visual effects
using Server.Spells.Fifth;     // for Meteor Swarm base, if desired
using Server.Spells.Seventh;   // for Chain Lightning inspiration

namespace Server.Mobiles
{
    [CorpseName("a hexa-seraph corpse")]
    public class HexaSeraph : BaseCreature
    {
        // Cooldown timers for its six powers
        private DateTime m_NextWingGust;
        private DateTime m_NextJudgement;
        private DateTime m_NextBindingChains;
        private DateTime m_NextSummonCast;
        private DateTime m_NextHealingPulse;
        private DateTime m_NextHolyFlare;

        // Unique golden-white hue
        private const int UniqueHue = 2401;

        [Constructable]
        public HexaSeraph()
            : base( AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name = "a Hexa-Seraph";
            Body = 0x109;             // use Hydra body
            BaseSoundID = 0x16A;      // use Hydra sounds
            Hue = UniqueHue;

            // Stats supercharged
            SetStr( 1000, 1100 );
            SetDex( 200, 250 );
            SetInt( 800, 900 );

            SetHits( 3000, 3300 );
            SetStam( 500, 600 );
            SetMana( 1500, 1800 );

            SetDamage( 25, 30 );      // physical claw swipes

            // Damage types: mostly holy/energy, some physical
            SetDamageType( ResistanceType.Physical, 20 );
            SetDamageType( ResistanceType.Energy, 80 );

            // Resistances
            SetResistance( ResistanceType.Physical, 60, 70 );
            SetResistance( ResistanceType.Fire,     50, 60 );
            SetResistance( ResistanceType.Cold,     50, 60 );
            SetResistance( ResistanceType.Poison,   40, 50 );
            SetResistance( ResistanceType.Energy,   85, 95 );

            // Skills
            SetSkill( SkillName.EvalInt,    120.0, 130.0 );
            SetSkill( SkillName.Magery,     120.0, 130.0 );
            SetSkill( SkillName.MagicResist,130.0, 140.0 );
            SetSkill( SkillName.Tactics,     95.0, 105.0 );
            SetSkill( SkillName.Wrestling,   95.0, 105.0 );
            SetSkill( SkillName.Meditation, 110.0, 120.0 );

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextWingGust     = now + TimeSpan.FromSeconds(10);
            m_NextJudgement    = now + TimeSpan.FromSeconds(15);
            m_NextBindingChains= now + TimeSpan.FromSeconds(20);
            m_NextSummonCast   = now + TimeSpan.FromSeconds(25);
            m_NextHealingPulse = now + TimeSpan.FromSeconds(30);
            m_NextHolyFlare    = now + TimeSpan.FromSeconds(35);

            // Loot
            PackGold( 2000, 3000 );
            AddLoot( LootPack.UltraRich, 2 );
            PackItem( new Diamond( Utility.RandomMinMax(5,10) ) );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Combatant == null || Map == null || !Alive )
                return;

            var now = DateTime.UtcNow;

            // 1) Wing Gust: cleave all in front in 3-tile cone, knockback+stun
            if ( now >= m_NextWingGust && InRange( Combatant.Location, 4 ) )
            {
                WingGust();
                m_NextWingGust = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12,18));
                return;
            }

            // 2) Heavenly Judgement: targeted orb rains down (single target AoE)
            if ( now >= m_NextJudgement && InRange( Combatant.Location, 12 ) )
            {
                HeavenlyJudgement();
                m_NextJudgement = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18,24));
                return;
            }

            // 3) Binding Chains: roots target and nearby in place, magic damage over time
            if ( now >= m_NextBindingChains && InRange( Combatant.Location, 8 ) )
            {
                BindingChains();
                m_NextBindingChains = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20,30));
                return;
            }

            // 4) Summon Celestial Hounds: 3 ethereal hounds attack for 20s
            if ( now >= m_NextSummonCast )
            {
                SummonHounds();
                m_NextSummonCast = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40,60));
                return;
            }

            // 5) Healing Pulse: ground tile that heals Hexa-Seraph and damages enemies
            if ( now >= m_NextHealingPulse )
            {
                HealingPulse();
                m_NextHealingPulse = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30,45));
                return;
            }

            // 6) Holy Flare: massive multi‑target beam from sky, heavy radiant damage
            if ( now >= m_NextHolyFlare && InRange( Combatant.Location, 16 ) )
            {
                HolyFlare();
                m_NextHolyFlare = now + TimeSpan.FromSeconds(Utility.RandomMinMax(60,80));
                return;
            }
        }

        // --- 1) Wing Gust: cone cleave + knockback + brief stun ---
        private void WingGust()
        {
            Say("*Be swept by my wings!*");
            PlaySound( 0x1F9 );
            var dir = GetDirectionTo( Combatant.Location );
            var affected = new List<Mobile>();

            // Collect targets in a 90° cone out to 3 tiles
            IPooledEnumerable eable = Map.GetMobilesInRange( Location, 3 );
            foreach ( Mobile m in eable )
            {
                if ( m != this && CanBeHarmful( m, false ) && SpellHelper.ValidIndirectTarget( this, m ) )
                {
                    // use Map.GetDistance and direction check

                }
            }
            eable.Free();

            foreach ( Mobile m in affected )
            {
                DoHarmful( m );
                AOS.Damage( m, this, Utility.RandomMinMax(40,60), 100, 0, 0, 0, 0 );

                // brief stun
                m.Frozen = true;
                Timer.DelayCall( TimeSpan.FromSeconds(1.5), () =>
                {
                    if ( m != null && !m.Deleted )
                        m.Frozen = false;
                } );

                // knockback
                PushBack( m, 2 );
            }
        }

        // --- 2) Heavenly Judgement: single-target orb that explodes in 3-tile AoE ---
        private void HeavenlyJudgement()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*By divine will!*");
            PlaySound( 0x3E9 );
            Point3D loc = target.Location;

            // missile effect from sky
            Effects.SendLocationEffect( loc, Map, 0x3709, 15, UniqueHue, 0 );
            Timer.DelayCall( TimeSpan.FromSeconds(1.0), () =>
            {
                // explosion
                Effects.PlaySound( loc, Map, 0x2E9 );
                Effects.SendLocationParticles( EffectItem.Create( loc, Map, EffectItem.DefaultDuration ),
                    0x36BD, 10, 30, UniqueHue, 0, 5034, 0 );

                // damage all in 3-tile radius
                IPooledEnumerable eable2 = Map.GetMobilesInRange( loc, 3 );
                foreach ( Mobile m in eable2 )
                {
                    if ( m != this && CanBeHarmful( m, false ) && SpellHelper.ValidIndirectTarget( this, m ) )
                    {
                        DoHarmful( m );
                        AOS.Damage( m, this, Utility.RandomMinMax(60,80), 0, 100, 0, 0, 0 );
                    }
                }
                eable2.Free();
            } );
        }

        // --- 3) Binding Chains: roots target + neighbors, ticking energy damage ---
        private void BindingChains()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*Bound by holy chains!*");
            PlaySound( 0x5C5 );

            var chained = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange( target.Location, 4 );
            foreach ( Mobile m in eable )
            {
                if ( m != this && CanBeHarmful( m, false ) && SpellHelper.ValidIndirectTarget( this, m ) )
                    chained.Add( m );
            }
            eable.Free();

            foreach ( Mobile m in chained )
            {
                // chain visual
                m.FixedParticles( 0x373A, 10, 30, 5030, UniqueHue, 0, EffectLayer.Waist );
                m.SendMessage( 0x48, "Divine chains bind you!" );

                // root
                m.Frozen = true;
                Timer.DelayCall( TimeSpan.FromSeconds(4.0), () =>
                {
                    if ( m != null && !m.Deleted )
                        m.Frozen = false;
                } );

                // damage over time
                Timer.DelayCall( TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 3, () =>
                {
                    if ( m != null && !m.Deleted && CanBeHarmful( m, false ) )
                    {
                        DoHarmful( m );
                        AOS.Damage( m, this, Utility.RandomMinMax(20,30), 0, 100, 0, 0, 0 );
                    }
                } );
            }
        }

        // --- 4) Summon Celestial Hounds: three ephemeral pets ---
        private void SummonHounds()
        {
            Say("*Arise, my guardians!*");
            PlaySound( 0x2E3 );

            for ( int i = 0; i < 3; i++ )
            {
                var hound = new HellHound(); // built‑in ethereal hound
                hound.Team = this.Team;
                hound.FightMode = FightMode.Closest;
                hound.Controlled = false;
                hound.MoveToWorld( new Point3D( X + Utility.RandomMinMax(-2,2), Y + Utility.RandomMinMax(-2,2), Z ), Map );

                // auto-delete after 20 seconds
                Timer.DelayCall( TimeSpan.FromSeconds(20.0 ), () =>
                {
                    if ( hound != null && !hound.Deleted )
                        hound.Delete();
                } );
            }
        }

        // --- 5) Healing Pulse: healing tile under self, damages enemies on contact ---
        private void HealingPulse()
        {
            Say("*I draw strength from the light!*");
            PlaySound( 0x1F4 );

            // drop a HealingPulseTile at self, transforms into AoE zone
            var tile = new Server.Items.HealingPulseTile
            {
                Hue = UniqueHue
            };
            tile.MoveToWorld( Location, Map );
        }

        // --- 6) Holy Flare: massive line of light hitting up to 6 targets ---
        private void HolyFlare()
        {
            Say("*Behold the sixfold light!*");
            PlaySound( 0x3F1 );

            var hits = 0;
            var potential = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange( Location, 16 );
            foreach ( Mobile m in eable )
            {
                if ( m != this && CanBeHarmful( m, false ) && SpellHelper.ValidIndirectTarget( this, m ) )
                    potential.Add( m );
            }
            eable.Free();

            while ( hits < 6 && potential.Count > 0 )
            {
                var idx = Utility.Random( potential.Count );
                var m = potential[idx];
                potential.RemoveAt(idx);

                // beam effect
                Effects.SendMovingParticles(
                    new Entity( Serial.Zero, Location, Map ),
                    new Entity( Serial.Zero, m.Location, Map ),
                    0x36BD, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100
                );

                Timer.DelayCall( TimeSpan.FromSeconds(0.2 * hits), () =>
                {
                    if ( m != null && !m.Deleted && CanBeHarmful( m, false ) )
                    {
                        DoHarmful( m );
                        AOS.Damage( m, this, Utility.RandomMinMax(50,75), 0, 100, 0, 0, 0 );
                    }
                } );

                hits++;
            }
        }

        public override void OnDeath( Container c )
        {
            base.OnDeath( c );
			if (Map == null) return;

            Say("*My light... fades...*");
            PlaySound( 0x212 );
            Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ),
                0x3709, 20, 60, UniqueHue, 0, 5052, 0 );

            // drop a few Divine Cores
            if ( Utility.RandomDouble() < 0.25 )
                c.DropItem( new PowerCrystal() );
        }

        // Standard overrides
        public override bool BleedImmune   => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public HexaSeraph( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }

        // --- Knock‑back helper ---
        private static readonly int[] OffsetX = { 0, 1, 1, 1, 0, -1, -1, -1 };
        private static readonly int[] OffsetY = { -1, -1, 0, 1, 1, 1, 0, -1 };

		private void PushBack( Mobile m, int distance )
		{
			if ( m == null || m.Map == null ) return;

			Direction dir = GetDirectionTo( m.Location );

			for ( int i = 0; i < distance; i++ )
			{
				var newX = m.X + OffsetX[(int)dir];
				var newY = m.Y + OffsetY[(int)dir];
				var newLoc = new Point3D( newX, newY, m.Z );

				if ( Map.CanFit( newLoc, 16, false, false ) )
				{
					m.Location = newLoc;
				}
				else
				{
					break;
				}
			}
		}

    }
}
