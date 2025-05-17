using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cursed solen warrior corpse")]
    public class CursedSolenWarrior : BaseCreature
    {
        private DateTime m_NextAcidBurst;
        private DateTime m_NextSummon;
        private const int UniqueHue = 1366; // Sickly violet-green

        [Constructable]
        public CursedSolenWarrior()
            : base( AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4 )
        {
            Name           = "a cursed solen warrior";
            Body           = 782;
            BaseSoundID    = 959;
            Hue            = UniqueHue;

            // ——— Stats ———
            SetStr( 300, 350 );
            SetDex( 120, 150 );
            SetInt( 80, 100 );

            SetHits( 600, 700 );
            SetStam( 200, 250 );
            SetMana( 150, 200 );

            SetDamage( 10, 20 );
            SetDamageType( ResistanceType.Physical, 60 );
            SetDamageType( ResistanceType.Poison,   20 );
            SetDamageType( ResistanceType.Energy,   20 );

            // ——— Resistances ———
            SetResistance( ResistanceType.Physical, 30, 40 );
            SetResistance( ResistanceType.Fire,     25, 35 );
            SetResistance( ResistanceType.Cold,     20, 30 );
            SetResistance( ResistanceType.Poison,   50, 60 );
            SetResistance( ResistanceType.Energy,   40, 50 );

            // ——— Skills ———
            SetSkill( SkillName.MagicResist,  90.0, 100.0 );
            SetSkill( SkillName.Tactics,      90.0, 100.0 );
            SetSkill( SkillName.Wrestling,    90.0, 100.0 );

            Fame       = 15000;
            Karma      = -15000;
            VirtualArmor = 60;
            ControlSlots = 4;

            // ——— Initial cooldowns ———
            m_NextAcidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8,12));
            m_NextSummon    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20,30));

            // ——— Loot ———
            PackItem( new ZoogiFungus( Utility.RandomMinMax(5,10) ) );
            PackItem( new BlackPearl( Utility.RandomMinMax(5,10) ) );
        }

        // — Necrotic Aura: drains life when anyone moves within 2 tiles
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            base.OnMovement(m, oldLoc);

            if ( Alive && m != this && m.Map == Map && m.InRange(Location, 2) && m.Alive && CanBeHarmful(m,false) )
            {
                if ( m is Mobile target )
                {
                    DoHarmful(target);
                    int drain = Utility.RandomMinMax(8, 16);
                    if ( target.Hits >= drain )
                    {
                        target.Hits -= drain;
                        Hits += drain;
                        target.SendMessage( 0x3F, "Dark tendrils snare you, stealing your life!" );
                        target.FixedParticles( 0x3709, 10, 15, 5030, UniqueHue, 0, EffectLayer.Head );
                        PlaySound( 0x208 );
                    }
                }
            }
        }

        // — Trigger acid burst when hit by spell
        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);
            BeginAcidBurst();
        }

        // — Trigger curse when hit in melee
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if ( attacker is Mobile target && Utility.RandomDouble() < 0.3 )
            {
                // Curse of Withering: -10 Str/Dex for 10s
                target.SendMessage( 0x22, "You feel your muscles wither under a dark curse!" );
                target.FixedParticles( 0x3728, 20, 30, 5052, UniqueHue, 0, EffectLayer.Waist );
                target.PlaySound( 0x1F8 );

                BuffInfo info = new BuffInfo(
                    BuffIcon.Clumsy, 
                    1075654, // icon
                    1075655, // label
                    TimeSpan.FromSeconds(10),
                    target );

            }

            BeginAcidBurst();
        }

        // — Periodic Acid Burst around self
        private void BeginAcidBurst()
        {
            if ( DateTime.UtcNow < m_NextAcidBurst ) return;
            m_NextAcidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12,18));

            PlaySound( 0x118 );
            FixedParticles( 0x36D4, 1, 10, 5034, UniqueHue, 0, EffectLayer.CenterFeet );

            // Delay the AoE
            Timer.DelayCall( TimeSpan.FromSeconds(0.5), () =>
            {
                if ( !Alive ) return;
                List<Mobile> list = new List<Mobile>();
                foreach ( Mobile m in Map.GetMobilesInRange(Location, 4) )
                    if ( m != this && CanBeHarmful(m,false) && SpellHelper.ValidIndirectTarget(this,m) )
                        list.Add(m);

                foreach ( Mobile m in list )
                {
                    DoHarmful(m);
                    m.Paralyze(TimeSpan.FromSeconds(1.5));
                    AOS.Damage( m, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 100, 0 );
                    m.ApplyPoison( this, Poison.Greater );
                }
            });
        }

        // — Summon two little cursed Solen every ~25s
        public override void OnThink()
        {
            base.OnThink();

            if ( Alive && DateTime.UtcNow >= m_NextSummon && Combatant != null && InRange(Combatant.Location, 8) )
            {
                m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20,30));
                for ( int i = 0; i < 2; i++ )
                {
                    CursedSolenShade shade = new CursedSolenShade();
                    shade.MoveToWorld( new Point3D( X + Utility.RandomList(-1,0,1), Y + Utility.RandomList(-1,0,1), Z ), Map );
                }
                Say( "*The cursed solen flesh thrashes and spawns a shrieking shade!*" );
                PlaySound( 0x29E );
            }
        }

        // — On death: scatter toxic ground
        public override void OnDeath( Container c )
        {
            base.OnDeath(c);

            if ( Map == null ) return;
            for ( int i = 0; i < 5; i++ )
            {
                int dx = Utility.RandomMinMax(-3,3), dy = Utility.RandomMinMax(-3,3);
                Point3D loc = new Point3D( X+dx, Y+dy, Z );
                if ( !Map.CanFit(loc.X,loc.Y,loc.Z,16,false,false) )
                    loc.Z = Map.GetAverageZ(loc.X,loc.Y);

                PoisonTile tile = new PoisonTile(); 
                tile.Hue = UniqueHue;
                tile.MoveToWorld( loc, Map );

            }
        }

        public override bool BleedImmune  { get { return true; } }
        public override Poison PoisonImmune{ get { return Poison.Lethal; } }
        public override int TreasureMapLevel{ get { return 5; } }

        public CursedSolenWarrior( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize(writer);
            writer.Write( (int)0 );
            writer.Write( m_NextAcidBurst );
            writer.Write( m_NextSummon );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextAcidBurst = reader.ReadDateTime();
            m_NextSummon    = reader.ReadDateTime();
        }
    }

    // — Simple summoned minion class
    public class CursedSolenShade : BaseCreature
    {
        [Constructable]
        public CursedSolenShade() : base(AIType.AI_Melee, FightMode.Closest, 8, 1, 0.2, 0.4)
        {
            Name = "a cursed solen shade";
            Body = 782;
            BaseSoundID = 959;
            Hue = 1366;

            SetStr(100,120);
            SetDex(80,100);
            SetInt(20,30);

            SetHits(150,180);
            SetDamage(5,10);

            SetDamageType(ResistanceType.Poison,100);

            SetResistance(ResistanceType.Physical,20,30);
            SetResistance(ResistanceType.Poison,40,50);

            SetSkill(SkillName.Tactics,50);
            SetSkill(SkillName.Wrestling,50);

            Fame = 2000;
            Karma = -2000;
            VirtualArmor = 20;
        }

        public CursedSolenShade(Serial serial) : base(serial) { }

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
