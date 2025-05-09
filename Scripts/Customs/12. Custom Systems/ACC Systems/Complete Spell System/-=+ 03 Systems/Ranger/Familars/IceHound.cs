using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.Ranger
{
	[CorpseName( "an ice hound corpse" )]
	public class IceHoundFamiliar : BaseFamiliar
	{
		public IceHoundFamiliar()
		{
			Name = "an ice hound";
			Body = 98;
			Hue = 1152;
			BaseSoundID = 229;

			SetStr( 80 );
			SetDex( 70 );
			SetInt( 30 );

			SetHits( 50 );
			SetStam( 60 );
			SetMana( 0 );

			SetDamage( 8, 15 );

			SetDamageType( ResistanceType.Cold, 100 );

			SetResistance( ResistanceType.Physical, 10, 15 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 99 );
			SetResistance( ResistanceType.Poison, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.Wrestling, 40.0 );
			SetSkill( SkillName.Tactics, 40.0 );

			ControlSlots = 1;

			AddItem( new LightSource() );
		}

		private DateTime m_NextFlare;

		public override void OnThink()
		{
			base.OnThink();

			if ( DateTime.Now < m_NextFlare )
				return;

			m_NextFlare = DateTime.Now + TimeSpan.FromSeconds( 5.0 + (25.0 * Utility.RandomDouble()) );

			this.FixedEffect( 0x37C4, 1, 12, 1109, 6 );
			this.PlaySound( 230 );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), new TimerCallback( Flare ) );
		}

		private void Flare()
		{
			Mobile caster = this.ControlMaster;

			if ( caster == null )
				caster = this.SummonMaster;

			if ( caster == null )
				return;

			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 5 ) )
			{
				if ( m.Player && m.Alive && !m.IsDeadBondedPet && m.Karma <= 0 )
					list.Add( m );
			}

			for ( int i = 0; i < list.Count; ++i )
			{
				Mobile m = (Mobile)list[i];
				bool friendly = true;

				for ( int j = 0; friendly && j < caster.Aggressors.Count; ++j )
					friendly = ( ((AggressorInfo)caster.Aggressors[j]).Attacker != m );

				for ( int j = 0; friendly && j < caster.Aggressed.Count; ++j )
					friendly = ( ((AggressorInfo)caster.Aggressed[j]).Defender != m );

				if ( friendly )
				{
					m.FixedEffect( 0x37C4, 1, 12, 1109, 3 ); // At player
					m.Mana += 1 - (m.Karma / 1000);
				}
			}
		}



		public IceHoundFamiliar( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
